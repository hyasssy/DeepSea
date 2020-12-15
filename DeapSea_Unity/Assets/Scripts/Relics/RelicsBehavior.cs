using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

public class RelicsBehavior : MonoBehaviour, IRelics
{
    Params _params;
    float _relicsAttachmentDuration;
    Vector3 _randomVec;
    Renderer _renderer;
    GenerateRelics _generateRelics;
    PhaseManager _phaseManager;
    private void Start() {
        Params param = Resources.Load<Params>("Params");
        _generateRelics = FindObjectOfType<GenerateRelics>();
        _phaseManager = FindObjectOfType<PhaseManager>();
        _relicsAttachmentDuration  = param.RelicsAttachmentDuration;
        float _rotateSpeed = param.RelicsRotateSpeed * Random.Range(0.1f, 1f);
        _randomVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * _rotateSpeed;
        _renderer = GetComponent<Renderer>();
        this.UpdateAsObservable().Subscribe(_ => SelfRotate());
        this.UpdateAsObservable().Subscribe(_ => SelfDestroy());
    }
    void SelfRotate(){
        if(_renderer.isVisible){
            transform.Rotate(_randomVec * Time.deltaTime);
        }
    }
    void SelfDestroy(){
        float range = _generateRelics.GenerateRange;
        float distanceToPlayer = (transform.position - Camera.main.transform.position).magnitude;
        if(distanceToPlayer > range){
            _generateRelics.ReduceCurrentRelicsAmount();
            Destroy(gameObject);
        }
    }
    public void Selected(){
        Move().Forget();
    }
    async UniTask Move(){
        float time = 0;
        float p = 0;
        Vector3 startPos = transform.position;
        Transform targetCore = GameObject.Find("Core").transform;
        while(time < _relicsAttachmentDuration){
            time += Time.deltaTime;
            p = time / _relicsAttachmentDuration;
            p = Easing.QuadOut(p, 1, 0, 1);
            transform.position = Vector3.Lerp(startPos, targetCore.position, p);
            await UniTask.Yield();
        }
        transform.position = targetCore.position;
        transform.parent = targetCore;
        _generateRelics.ReduceCurrentRelicsAmount();
        _phaseManager.AddHoldAmount();
    }
}
