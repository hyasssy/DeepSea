using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SeaParamManager : MonoBehaviour
{
    [SerializeField]
    GameObject _sea;
    ReactiveProperty<float> currentWaterDepth = new ReactiveProperty<float>();
    Params _params;
    public float CurrentWaterDepth{
        get{return currentWaterDepth.Value;}
        private set{currentWaterDepth.Value = value;}
    }
    public float CurrentTopRange{get; private set;}
    public float CurrentBottomRange{get; private set;}
    void Start()
    {
        _params = Resources.Load<Params>("Params");
        Vector2 bowlSize = _params.BowlSize;
        CurrentWaterDepth = _params.WaterParamPack[0].waterDepth;
        CurrentTopRange = _params.WaterParamPack[0].topRange;
        CurrentBottomRange = _params.WaterParamPack[0].bottomRange;
        _sea.transform.localScale = new Vector3(bowlSize.x/10, CurrentWaterDepth/10, bowlSize.y/10);
        currentWaterDepth.Subscribe(_ => _sea.transform.localScale = new Vector3(bowlSize.x/10, CurrentWaterDepth/10, bowlSize.y/10));
    }

    async UniTask ChangeWaterDepthTask(int newPhase, float duration){
        float currentDepth = CurrentWaterDepth;
        float currentTopRange = CurrentTopRange;
        float currentBottomRange = CurrentBottomRange;
        float time = 0;
        while(time < duration){
            time += Time.deltaTime;
            float p = time / duration;
            p = Easing.QuadInOut(p,1,0,1);
            CurrentWaterDepth = Mathf.Lerp(currentDepth, _params.WaterParamPack[newPhase].waterDepth, p);
            CurrentTopRange = Mathf.Lerp(currentTopRange, _params.WaterParamPack[newPhase].topRange, p);
            CurrentBottomRange = Mathf.Lerp(currentBottomRange, _params.WaterParamPack[newPhase].bottomRange, p);
            await UniTask.Yield();
        }
    }
    public void ChangeWaterDepth(int newPhase, float duration){
        ChangeWaterDepthTask(newPhase, duration).Forget();
    }
}
