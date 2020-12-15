using UnityEngine;
public class LightIntensityShift : MonoBehaviour
{
    #region parameter
    [SerializeField]
    Color topColor = new Color(255,244,214,255), bottomColor = new Color(160, 251,255,255);
    [SerializeField]
    float topIntensity = 1.4f, bottomIntensity = 0.6f;
    [SerializeField]
    GameObject sceneLight;
    #endregion

    Light m_light;
    SeaParamManager _seaParamManager;
    Transform playercam;
    private void Start() {
        m_light = sceneLight.GetComponent<Light>();
        _seaParamManager = FindObjectOfType<SeaParamManager>();
        playercam = Camera.main.transform;
    }
    private void Update() {
        var p = -playercam.position.y / _seaParamManager.CurrentWaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        var targetIntensity = Mathf.Lerp(topIntensity, bottomIntensity, p);
        m_light.color = targetColor;
        m_light.intensity = targetIntensity;
    }

    private void Reset() {//シーン上で調節した時に、コンポーネントをリセットすると自動取得する。
        m_light = sceneLight.GetComponent<Light>();
        playercam = Camera.main.transform;
        var p = -playercam.position.y / _seaParamManager.CurrentWaterDepth;//水面から水深に対する現在の深さのパラメータ
        var targetColor = Color.Lerp(topColor, bottomColor, p);
        var targetIntensity = Mathf.Lerp(topIntensity, bottomIntensity, p);
        m_light.color = targetColor;
        m_light.intensity = targetIntensity;
    }
}
