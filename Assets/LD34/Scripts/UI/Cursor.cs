using System;
using UnityEngine;

namespace LD34.UI {

    public class Cursor : MonoBehaviour, IPulseListener {

        public Color normalColor = Color.white.WithA(0.5f);
        public Color hitColor = Color.green.WithR(0.5f);
        public Color activeColor = Color.green.WithR(0.85f);
        public Color missColor = Color.red;

        public float tintSharpness = 10f;

        private CanvasRenderer ren;
        private Color color, desiredColor;

        private void Awake() {
            this.CacheComponent(ref ren);
            desiredColor = normalColor;
        }

        public void ActivatePulse(float timing) {
            color = hitColor;
            desiredColor = activeColor;
        }

        public void FailPulse() {
            color = missColor;
            desiredColor = normalColor;
        }

        public void FinishPulse(float timing) {
            color = hitColor;
            desiredColor = normalColor;
        }

        public void MissPulse() {
            color = missColor;
            desiredColor = normalColor;
        }

        public void UpdatePulseProximity(float dt) {
            //ren.SetAlpha(Mathf.Clamp01(dt));
        }

        private void Update() {
            color = Color.Lerp(color, desiredColor, tintSharpness * Time.deltaTime);
            ren.SetColor(color);
        }
    }
}
