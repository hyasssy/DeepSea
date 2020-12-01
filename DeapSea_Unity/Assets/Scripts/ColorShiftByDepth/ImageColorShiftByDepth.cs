using UnityEngine;
using UnityEngine.UI;
public class ImageColorShiftByDepth : MonoBehaviour
{
    #region parameter
    public Color topColor;
    public Color bottomColor;
    public GameObject obj;
    #endregion

    
    Image imageComponent;
    Params _params;
    Transform playercam;

    void Start()
    {
        _params = Resources.Load<Params>("WaterParams");
        playercam = Camera.main.transform;
        imageComponent = obj.GetComponent<Image>();
    }

    void Update()
    {
        var p = -playercam.position.y / _params.WaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        imageComponent.color = targetColor;
    }

    private void Reset() {//シーン上で調節した時に、コンポーネントをリセットすると自動取得する。
        _params = Resources.Load<Params>("WaterParams");
        playercam = Camera.main.transform;
        imageComponent = obj.GetComponent<Image>();
        var p = -playercam.position.y / _params.WaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        imageComponent.color = targetColor;
    }
}
