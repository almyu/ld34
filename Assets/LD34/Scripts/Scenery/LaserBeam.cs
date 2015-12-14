using UnityEngine;

namespace LD34 {

    public class LaserBeam : MonoBehaviour {

        public SpriteRenderer muzzle, beam;
        public float fadeTime = 0.5f;

        private float fade;
        private bool fading;

        private void Awake() {
            enabled = false;
        }

        public void FadeOut() {
            enabled = true;
            fade = fadeTime;
        }

        private void Update() {
            var alpha = fade / fadeTime;
            fade -= Time.deltaTime;

            muzzle.color = muzzle.color.WithA(alpha);
            beam.color = beam.color.WithA(alpha);
        }

        public void SetLength(float length) {
            beam.transform.localScale = beam.transform.localScale.WithX(length * beam.sprite.pixelsPerUnit);
        }
    }
}
