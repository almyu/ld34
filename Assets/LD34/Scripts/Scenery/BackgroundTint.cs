using UnityEngine;

namespace LD34 {

    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundTint : MonoBehaviour {

        public Gradient gradient;
        public float tintSpeed = 0.125f;
        public float tintOffset = 0f;
        public float minAlpha = 0.5f;
        public float maxAlpha = 1f;

        private SpriteRenderer ren;
        private float gradPos;

        private void Awake() {
            this.CacheComponent(ref ren);
            gradPos = tintOffset;
        }

        private void Update() {
            gradPos = Mathf.Repeat(gradPos + tintSpeed * Time.deltaTime, 1f);

            var beatFlash = Mathf.Abs(TimeSync.time - TimeSync.CeilToBeat(TimeSync.time));
            beatFlash = Mathf.Lerp(minAlpha, maxAlpha, beatFlash);

            ren.color = gradient.Evaluate(gradPos) * beatFlash;
        }
    }
}
