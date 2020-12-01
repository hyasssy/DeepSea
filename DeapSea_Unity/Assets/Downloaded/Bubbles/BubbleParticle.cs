using UnityEngine;

//一つ一つのパーティクルの動きを制御。水面より上に出たら消す。
public class BubbleParticle : MonoBehaviour {
    ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;
    [SerializeField]
    float _vanishY = 0f;

    private void Start () {
        particleSystem = GetComponent<ParticleSystem> ();
    }

    private void Update () {
        int maxParticles = particleSystem.main.maxParticles;
        if (particles == null || particles.Length < maxParticles) { //数がmaxでかわらないうちは同じだよね、っていう負荷軽減処理
            particles = new ParticleSystem.Particle[maxParticles];
        }

        int particleNum = particleSystem.GetParticles (particles); //現在生存しているパーティクルの数
        for (int i = 0; i < particleNum; i++) {
            float tmpY = particles[i].position.y;
            if (tmpY >= _vanishY) {
                particles[i].remainingLifetime = 0;
            }
        }

        particleSystem.SetParticles (particles, particleNum); //セットする必要があるらしい。
    }
}