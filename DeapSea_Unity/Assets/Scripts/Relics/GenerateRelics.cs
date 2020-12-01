using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRelics : MonoBehaviour
{
    //Instantiate Relics around Player
    [SerializeField]
    GameObject[] _relics;
    [SerializeField]
    int _maxAmount = 100;
    [SerializeField]
    float _generateRange = 22f;
    Transform _player;
    void Start()
    {
        _player = FindObjectOfType<CamController>().transform;
        InstantiateRelicsAroundPlayer();
    }

    void InstantiateRelicsAroundPlayer(){
        Params param = Resources.Load<Params>("Params");
        for (int i = 0; i < _maxAmount;i++){
            float verticalTarget = Random.Range(-param.WaterDepth + param.BottomRange, -param.TopRange);
            Vector3 targetPos = _player.position + new Vector3(Random.Range(-1f, 1f)*_generateRange, 0, Random.Range(-1f, 1f) *_generateRange);
            targetPos.y = verticalTarget;
            GameObject newOb = Instantiate(_relics[Random.Range(0, _relics.Length)], targetPos, Random.rotation);
            newOb.transform.localScale =
            new Vector3(newOb.transform.localScale.x * Random.Range(0.5f, 2f),
            newOb.transform.localScale.y * Random.Range(0.5f, 2f),
            newOb.transform.localScale.z * Random.Range(0.5f, 2f));
        }
    }
}
