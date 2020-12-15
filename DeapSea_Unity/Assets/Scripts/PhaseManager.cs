using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PhaseManager : MonoBehaviour
{
    public int CurrentPhaseNumber {get;private set;} = 0;
    public int HoldAmount {get; private set;} = 0;
    Params _params;
    SeaParamManager _seaParamManager;
    Transform _mainPlayerCam;
    private void Start() {
        _params = Resources.Load<Params>("Params");
        _seaParamManager = FindObjectOfType<SeaParamManager>();
        _mainPlayerCam = Camera.main.transform;
        _mainPlayerCam.localPosition = new Vector3(0,0,_params.WaterParamPack[0].camPosition);
    }
    void NextPhase(){
        CurrentPhaseNumber ++;
        _seaParamManager.ChangeWaterDepth(CurrentPhaseNumber, 2f);
        MoveCamPos().Forget();
    }
    public void AddHoldAmount(){
        HoldAmount++;
        if(HoldAmount >= _params.WaterParamPack[CurrentPhaseNumber].phase_relicsAmount){
            NextPhase();
        }
    }
    async UniTask MoveCamPos(){
        float time = 0;
        float duration = 1.5f;
        float targetVecZ = _params.WaterParamPack[CurrentPhaseNumber].camPosition;
        while(time < duration){
            time += Time.deltaTime;
            float p = Easing.QuadInOut(time/duration,1,0,1);
            float value = Mathf.Lerp(_mainPlayerCam.localPosition.z, targetVecZ, p);
            _mainPlayerCam.localPosition = new Vector3(0,0,value);
            await UniTask.Yield();
        }
    }
}
