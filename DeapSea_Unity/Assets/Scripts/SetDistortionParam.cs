using UnityEngine;

//揺らすフィルター　酔うなら消す
public class SetDistortionParam : MonoBehaviour
{
    [SerializeField] float shiftSpeed = 0.1f;
    [SerializeField] Material material;

    void Update()
    {
        Vector2 targetPos = new Vector2(Time.time * shiftSpeed, Time.time * shiftSpeed);
        material.SetTextureOffset("_DistortionTex", targetPos);
    }
}
