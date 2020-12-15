using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GenerateRelics : MonoBehaviour
{
    //Instantiate Relics around Player
    public ReactiveProperty<int> CurrentRelicsAmount {get; private set;} = new ReactiveProperty<int>(0);
    public void ReduceCurrentRelicsAmount(){
        CurrentRelicsAmount.Value--;
    }

    [SerializeField]
    int _maxAmount = 100;
    [field: SerializeField, RenameField(nameof(GenerateRange))]
    public float GenerateRange {get; private set;} = 22f;
    Transform _player;
    Params _params;
    SeaParamManager seaParamManager;
    PhaseManager _phaseManager;
    void Start()
    {
        _params = Resources.Load<Params>("Params");
        _phaseManager = FindObjectOfType<PhaseManager>();
        _player = FindObjectOfType<CamController>().transform;
        seaParamManager = FindObjectOfType<SeaParamManager>();
        while(CurrentRelicsAmount.Value < _maxAmount){
            InstantiateRelicsAroundPlayer(true);
        }
        CurrentRelicsAmount.Subscribe(_ => InstantiateRelicsAroundPlayer(false));//CurrentAmountの値が変わった時に、呼び出す。
    }

    void InstantiateRelicsAroundPlayer(bool isInitialize){
        if(CurrentRelicsAmount.Value >= _maxAmount) return;
        float verticalTarget = Random.Range(seaParamManager.CurrentBottomRange, seaParamManager.CurrentTopRange);
        Vector3 targetPos = _player.position + new Vector3(Random.Range(-1f, 1f)*GenerateRange, 0, Random.Range(-1f, 1f) *GenerateRange);
        if(!isInitialize)targetPos = targetPos.normalized * GenerateRange * 0.95f;
        targetPos.y = verticalTarget;
        int relicsArrayLength = _params.WaterParamPack[_phaseManager.CurrentPhaseNumber].relics.Length;
        GameObject newOb = Instantiate(_params.WaterParamPack[_phaseManager.CurrentPhaseNumber].relics[Random.Range(0, relicsArrayLength)], targetPos, Random.rotation);
        newOb.transform.localScale =
        new Vector3(newOb.transform.localScale.x * Random.Range(0.5f, 2f),
        newOb.transform.localScale.y * Random.Range(0.5f, 2f),
        newOb.transform.localScale.z * Random.Range(0.5f, 2f));
        CurrentRelicsAmount.Value++;
    }
}
