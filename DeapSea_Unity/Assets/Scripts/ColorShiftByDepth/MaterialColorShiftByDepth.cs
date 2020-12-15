using UnityEngine;

public class MaterialColorShiftByDepth : MonoBehaviour
{//水面から見たらオレンジだけど底にいったら青く見えるというのは本来的にはありえないけど、多分その方が見え方面白いからそうしてみる。
    #region parameter
    [SerializeField]
    Color topColor = new Color(233,212,173,255);
    [SerializeField]
    Color bottomColor = new Color(84,81,166,255);
    [SerializeField]
    GameObject obj;
    #endregion

    Material material;
    SeaParamManager _seaParamManager;
    Transform playercam;

    void Start()
    {
        _seaParamManager = FindObjectOfType<SeaParamManager>();
        playercam = Camera.main.transform;
        material = obj.GetComponent<Renderer>().material;
    }

    void Update()
    {
        var p = Mathf.Abs(playercam.position.y) / _seaParamManager.CurrentWaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        material.color = targetColor;
    }

    private void Reset() {//シーン上で調節した時に、コンポーネントをリセットすると自動取得する。
        _seaParamManager = FindObjectOfType<SeaParamManager>();
        playercam = Camera.main.transform;
        material = obj.GetComponent<Renderer>().material;
        var p = -playercam.position.y / _seaParamManager.CurrentWaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        material.color = targetColor;
    }
}
