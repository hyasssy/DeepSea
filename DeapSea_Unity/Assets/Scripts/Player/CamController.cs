using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CamController : MonoBehaviour//for debug
{
    [SerializeField]
    Transform _seaContainer;//海をカメラに追随して動かすことによって、無限の海にする。
    [SerializeField]
    Transform _castle;//遠くに見える建造物
    Transform _playerCam;
    SeaParamManager _seaParamManager;
    KeyCode[] _movekeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E};
    KeyCode[] _rotatekeys = {KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    public float _moveSpeed = 1f;
    public float _rotateSpeed = 10f;
    public float _rotateAcceleration = 20f;
    public float _goForwardSpeed = 0.2f;
    float[] _tempRotateSpeed = {0,0};//{vertical, horizontal}

    private void Start() {
        Params waterParams = Resources.Load<Params>("Params");
        _seaParamManager = FindObjectOfType<SeaParamManager>();
        _playerCam = Camera.main.transform;
        this.UpdateAsObservable().Subscribe(_ => GoForward());
    }

    void GoForward(){
        AcceleratedRotate();
        Vector3 targetVec = _playerCam.forward * _goForwardSpeed * Time.deltaTime;
        transform.position += targetVec;
        RestrictRange();
        Vector3 horizontalVec = Vector3.Scale(targetVec, new Vector3(1,0,1));//高さは変えず
        _seaContainer.position += horizontalVec;//海をプレイヤーに追随させる
        _castle.position += horizontalVec;//城を追随させる
    }

    void RestrictRange(){
        float currentY = _playerCam.transform.position.y;
        if(currentY > _seaParamManager.CurrentTopRange){
            transform.position -= new Vector3(0,currentY - _seaParamManager.CurrentTopRange,0);//プレイヤーの親オブジェを下げることで対応。
        }else if(currentY < _seaParamManager.CurrentBottomRange){
            transform.position -= new Vector3(0, currentY - _seaParamManager.CurrentBottomRange, 0);
        }
    }

    /*void OptionalControl(){
        int[] moveInputs = {0,0,0,0,0,0};
        for(int i=0;i<6;i++){
            if(Input.GetKey(_movekeys[i])){
                moveInputs[i]++;
            }
        }
        Vector3 horizontalDis = Vector3.Scale(transform.forward, new Vector3(1,0,1)) * (moveInputs[0]-moveInputs[2]) * _moveSpeed * Time.deltaTime + transform.right * (moveInputs[3]-moveInputs[1]) * _moveSpeed * Time.deltaTime;
        Vector3 verticalDis = Vector3.up * (moveInputs[4]-moveInputs[5]) * _moveSpeed * Time.deltaTime;
        transform.position += horizontalDis + verticalDis;
        _seaContainer.position += horizontalDis;//海も水平方向にカメラを追随。
        AcceleratedRotate();
    }*/


    void AcceleratedRotate(){
        _tempRotateSpeed[0] = Accelerate(_tempRotateSpeed[0],_rotatekeys[2], _movekeys[2], _rotatekeys[0], _movekeys[0], _rotateSpeed, _rotateAcceleration);
        _tempRotateSpeed[1] = Accelerate(_tempRotateSpeed[1],_rotatekeys[3], _movekeys[3], _rotatekeys[1], _movekeys[1], _rotateSpeed, _rotateAcceleration);
        transform.localEulerAngles += new Vector3(_tempRotateSpeed[0] * Time.deltaTime, _tempRotateSpeed[1] * Time.deltaTime, 0);
    }

    /// <summary>
    /// 加速度的にspeedをコントロール
    /// </summary>
    /// <param name="keyPosi1">キーの割当。kay1-key2で正負判断</param>
    float Accelerate(float speed, KeyCode keyPosi1, KeyCode keyPosi2, KeyCode keyNega1, KeyCode keyNega2, float maxSpeed, float acceleration){
        float tempAcceleration = acceleration * Time.deltaTime;//そのフレームの加速量
        bool posiInput = !Input.GetKey(keyPosi1) && !Input.GetKey(keyPosi2) ? false : true;
        bool negaInput = !Input.GetKey(keyNega1) && !Input.GetKey(keyNega2) ? false : true;
        if(posiInput && negaInput){//両方押してるとき
            //何もしない
        }else if(posiInput){//ポジティブだけ押してるとき
            if(speed < maxSpeed){
                speed += tempAcceleration;
            }
        }else if(negaInput){//ネガティブだけ押してる
            if(speed > -maxSpeed){
                speed -= tempAcceleration;
            }
        }else{//該当方向の入力なし
            if(Mathf.Abs(speed) < tempAcceleration){
                speed = 0;
            }else if(speed > 0){
                speed -= tempAcceleration;
            }else{
                speed += tempAcceleration;
            }
        }
        return speed;
    }
}
