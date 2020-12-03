using UnityEngine;

public class GeneralSettings : MonoBehaviour
{
    void Start()
    {
        SetParams();
    }
    private void Reset() {
        SetParams();
    }
    void SetParams(){
        Params param = Resources.Load<Params>("Params");
        Debug.Log("SetParams()");
        GameObject.Find("MainCanvas").transform.Find("DebugLogObject").gameObject.SetActive(param.IsDisplayConsolePanel);
        GameObject.Find("Sea").transform.localScale = new Vector3(param.BowlSize.x/10, param.WaterDepth/10, param.BowlSize.y/10);
    }
}
