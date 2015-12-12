using UnityEngine;
using System.Collections;

namespace LD34.UI {

    [RequireComponent(typeof(CanvasRenderer))]
    public class Blinker : MonoBehaviour {

        public Gradient gradient;
        public float duration = 0.2f;
        public bool normallyTransparent;

        private CanvasRenderer cachedRenderer;

        private void Awake() {
            cachedRenderer = GetComponent<CanvasRenderer>();
        }

        private void Start() {
            if (normallyTransparent) cachedRenderer.SetColor(Color.clear);
        }

        public void Blink() {
            StartCoroutine(DoBlink());
        }

        private IEnumerator DoBlink() {
            for (var t = 0f; t <= duration; t += Time.deltaTime) {
                cachedRenderer.SetColor(gradient.Evaluate(t / duration));
                yield return null;
            }
            if (normallyTransparent) cachedRenderer.SetColor(Color.clear);
        }
    }
}
