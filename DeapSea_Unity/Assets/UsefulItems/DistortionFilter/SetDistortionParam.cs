using UnityEngine;
using System.Collections;

public class SetDistortionParam : MonoBehaviour
{
    [SerializeField] float shiftSpeed = 0.1f;
    [SerializeField] Material material;

    private void Start()
    {
        StartCoroutine(ShiftDistortionParam());
    }
    IEnumerator ShiftDistortionParam()
    {
        while (true)
        {
            Vector2 targetPos = Vector2.one * Time.time * shiftSpeed;
            material.SetTextureOffset("_DistortionTex", targetPos);//materialの情報を強引に書き換えることにしてる。
            yield return null;
        }
    }
}
