using UnityEngine;
public class LightIntensityShift : MonoBehaviour
{
    #region parameter
    [SerializeField]
    Color topColor, bottomColor;
    [SerializeField]
    float topIntensity, bottomIntensity;
    [SerializeField]
    GameObject sceneLight;
    #endregion

    Light m_light;
    Params _params;
    Transform playercam;
    private void Start() {
        m_light = sceneLight.GetComponent<Light>();
        _params = Resources.Load<Params>("Params");
        playercam = Camera.main.transform;
    }
    private void Update() {
        var p = -playercam.position.y / _params.WaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        var targetIntensity = Mathf.Lerp(topIntensity, bottomIntensity, p);
        m_light.color = targetColor;
        m_light.intensity = targetIntensity;
    }

    private void Reset() {//シーン上で調節した時に、コンポーネントをリセットすると自動取得する。
        m_light = sceneLight.GetComponent<Light>();
        _params = Resources.Load<Params>("Params");
        playercam = Camera.main.transform;
        var p = -playercam.position.y / _params.WaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        var targetIntensity = Mathf.Lerp(topIntensity, bottomIntensity, p);
        m_light.color = targetColor;
        m_light.intensity = targetIntensity;
    }
}
