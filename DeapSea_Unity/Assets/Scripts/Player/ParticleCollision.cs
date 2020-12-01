using UnityEngine;

public class ParticleCollision : MonoBehaviour
{

    private void Start() {
    }

    void OnParticleCollision(GameObject obj){
        Debug.Log("衝突しているのは"+obj.name);
        IRelics iRelics = obj.GetComponent<IRelics>();
        if(iRelics != null){
            iRelics.Selected();
        }
    }
}
