using UnityEngine;
using System.Collections;

namespace LD34.UI {

    [RequireComponent(typeof(CanvasRenderer))]
    public class Blinker : MonoBehaviour {

        public Gradient gradient;
        public float duration = 0.2f;

        private CanvasRenderer cachedRenderer;

        private void Awake() {
            cachedRenderer = GetComponent<CanvasRenderer>();
        }

        private void Start() {
            cachedRenderer.SetColor(gradient.Evaluate(1f));
        }

        public void Blink() {
            StartCoroutine(DoBlink());
        }

        private IEnumerator DoBlink() {
            for (var t = 0f; t <= duration; t += Time.deltaTime) {
                cachedRenderer.SetColor(gradient.Evaluate(t / duration));
                yield return null;
            }
            cachedRenderer.SetColor(gradient.Evaluate(1f));
        }
    }
}
