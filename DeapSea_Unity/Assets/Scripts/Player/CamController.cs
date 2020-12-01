using UnityEngine;

public class CamController : MonoBehaviour//for debug
{
    [Tooltip("true:正面に向かって進む、false:上下前後左右に操作できる")]
    public bool goForwardOrNot = false;
    [SerializeField]
    Transform _seaContainer;//Aquasオブジェをカメラに追随して動かすことによって、無限の海にする。
    Transform _playerCam;
    float _topRange;
    float _bottomRange;

    KeyCode[] _movekeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E};
    KeyCode[] _rotatekeys = {KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    public float _moveSpeed = 1f;
    public float _rotateSpeed = 10f;
    public float _goForwardSpeed = 0.2f;
    
    private void Start() {
        Params waterParams = Resources.Load<Params>("WaterParams");
        _playerCam = Camera.main.transform;
        _bottomRange = waterParams.WaterDepth - waterParams.BottomRange;//水深3mなら3（-3じゃない。）
        _topRange = waterParams.TopRange;
    }
    private void Update() {
        if(goForwardOrNot){
            GoForward();
        }else{
            OptionalControl();
        }
    }

    void GoForward(){
        Rotate();
        Vector3 targetVec = _playerCam.forward * _goForwardSpeed * Time.deltaTime;
        transform.position += targetVec;
        float currentY = _playerCam.transform.position.y;
        if(currentY > _topRange){
            transform.position -= new Vector3(0,currentY - _topRange,0);//プレイヤーの親オブジェを下げることで対応。
        }else if(currentY < -_bottomRange){
            transform.position -= new Vector3(0, currentY - -_bottomRange, 0);
        }
        Vector3 horizontalVec = Vector3.Scale(targetVec, new Vector3(1,0,1));//高さは変えず
        _seaContainer.position += horizontalVec;
    }

    void OptionalControl(){
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
        Rotate();
    }

    void Rotate(){
        int[] rotateInputs = {0,0,0,0};
        for(int i=0;i<4;i++){
            if(Input.GetKey(_rotatekeys[i])){
                rotateInputs[i]++;
            }
        }
        transform.localEulerAngles += new Vector3((rotateInputs[2]-rotateInputs[0]) * _rotateSpeed * Time.deltaTime, (rotateInputs[3]-rotateInputs[1]) * _rotateSpeed * Time.deltaTime, 0);
    }
}
