using UnityEngine;

public class MaterialColorShiftByDepth : MonoBehaviour
{//水面から見たらオレンジだけど底にいったら青く見えるというのは本来的にはありえないけど、多分その方が見え方面白いからそうしてみる。
    #region parameter
    [SerializeField]
    Color topColor;
    [SerializeField]
    Color bottomColor;
    [SerializeField]
    GameObject obj;
    #endregion

    Material material;
    Params _params;
    Transform playercam;
    float _waterDepth;

    void Start()
    {
        _params = Resources.Load<Params>("WaterParams");
        playercam = Camera.main.transform;
        material = obj.GetComponent<Renderer>().material;
    }

    void Update()
    {
        _waterDepth = _params.WaterDepth;
        var p = Mathf.Abs(playercam.position.y) / _waterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        material.color = targetColor;
    }

    private void Reset() {//シーン上で調節した時に、コンポーネントをリセットすると自動取得する。
        _params = Resources.Load<Params>("WaterParams");
        playercam = Camera.main.transform;
        material = obj.GetComponent<Renderer>().material;
        var p = -playercam.position.y / _params.WaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        material.color = targetColor;
    }
}
