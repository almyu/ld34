using UnityEngine;

namespace LD34 {

    public class LaserBeam : MonoBehaviour {

        public SpriteRenderer muzzle, beam;
        public float lifetime = 5f;

        private float life;

        private void Awake() {
            Destroy(gameObject, lifetime);
            life = lifetime;
        }

        private void Update() {
            var alpha = life / lifetime;
            life -= Time.deltaTime;

            muzzle.color = muzzle.color.WithA(alpha);
            beam.color = beam.color.WithA(alpha);
        }

        public void SetLength(float length) {
            beam.transform.localScale = new Vector3(length, 1, 1);
        }
    }
}
