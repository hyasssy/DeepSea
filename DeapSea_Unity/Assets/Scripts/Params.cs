using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create Params")]
public class Params : ScriptableObject
{
    [field: SerializeField, RenameField(nameof(TopRange))]
    public float TopRange {get; private set;} = 0.2f;
    [field: SerializeField, RenameField(nameof(BottomRange))]
    public float BottomRange = 0.7f;
    [field: SerializeField, RenameField(nameof(WaterDepth))]
    public float WaterDepth {get; private set;} = 10f;
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
        GameObject.Find("Sea").transform.localScale = new Vector3(BowlSize.x/10, WaterDepth/10, BowlSize.y/10);
    }
}
