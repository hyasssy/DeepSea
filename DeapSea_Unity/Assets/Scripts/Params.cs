using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "MyScriptable/Create Params")]
public class Params : ScriptableObject
{
    [System.Serializable]
    public class WaterParams{
        public float topRange;
        public float bottomRange;
        public float waterDepth;
        public GameObject[] relics;
        public int phase_relicsAmount;
        public float camPosition;
    }

    [field: SerializeField]
    public List<WaterParams> WaterParamPack{get;private set;}
    [field: SerializeField, RenameField(nameof(BowlSize))]
    public Vector2 BowlSize {get; private set;} = new Vector2(50f, 50f);
    [field: SerializeField, RenameField(nameof(RelicsAttachmentDuration))]
    public float RelicsAttachmentDuration {get; private set;} = 1f;
    [field: SerializeField, RenameField(nameof(RelicsRotateSpeed))]
    public float RelicsRotateSpeed {get; private set;} = 1f;
    [SerializeField]
    bool _isDisplayConsolePanel = true;

    private void OnValidate() {
        Debug.Log("Params>>OnValidate Called");
        GameObject.Find("MainCanvas").transform.Find("DebugLogObject").gameObject.SetActive(_isDisplayConsolePanel);
        GameObject.Find("Sea").transform.localScale = new Vector3(BowlSize.x/10, WaterParamPack[0].waterDepth/10, BowlSize.y/10);
    }
}

