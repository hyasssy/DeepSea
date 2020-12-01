using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ShootSelect : MonoBehaviour
{
    Camera _playerCam;
    [SerializeField]
    ParticleSystem bulletParticle;
    private void Start() {
        _playerCam = Camera.main;
        this.UpdateAsObservable().Subscribe(_ => Click());
    }
    private void Update() {

    }
    void Click(){//Debug時にマウスで選択
        if(Input.GetMouseButtonDown(0)){
            Vector3 startPos = _playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _playerCam.nearClipPlane));
            Vector3 axis = (startPos - _playerCam.transform.position).normalized;
            ShootParticle(startPos, axis);
        }
    }

    void ShootParticle(Vector3 startPos, Vector3 axis){
        bulletParticle.transform.position = startPos;
        bulletParticle.transform.forward = axis;
        bulletParticle.time = 0;
        bulletParticle.Play();
    }
}
