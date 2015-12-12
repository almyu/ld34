using UnityEngine;
using System.Collections.Generic;

namespace LD34.UI {

    [RequireComponent(typeof(RectTransform))]
    public class TimelineUI : MonoBehaviour {

        public RectTransform pulseTemplate;
        public Timeline timeline;
        public float pixelsPerSecond = 100f;

        private RectTransform currentPulse;
        private Queue<RectTransform> pulses = new Queue<RectTransform>();

        private void Update() {
            AnimatePulses();
            if (currentPulse) GrowPulse();
        }

        public void StartPulse() {
            currentPulse = Instantiate(pulseTemplate);
            currentPulse.anchoredPosition = Vector2.zero;
            currentPulse.SetParent(transform, false);
            currentPulse.gameObject.SetActive(true);
            pulses.Enqueue(currentPulse);
        }

        public void AnimatePulses() {
            foreach (RectTransform child in transform)
                if (child.gameObject.activeSelf)
                    child.anchoredPosition += Vector2.left * pixelsPerSecond * Time.deltaTime;
        }

        public void GrowPulse() {
            currentPulse.sizeDelta += Vector2.right * pixelsPerSecond * Time.deltaTime;
        }

        public void EndPulse() {
            currentPulse = null;
        }

        private static Gradient MakeBlinkGradient(Color color) {
            return new Gradient {
                colorKeys = new[] { new GradientColorKey(color, 0f), new GradientColorKey(Color.clear, 1f) },
                alphaKeys = new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
            };
        }

        public void DiscardPulse(bool hit) {
            if (pulses.Count == 0) return;

            var top = pulses.Dequeue();
            var blinker = top.gameObject.AddComponent<Blinker>();

            blinker.gradient = MakeBlinkGradient(hit ? Color.green : Color.red);
            blinker.Blink();

            Destroy(top.gameObject, blinker.duration);
        }
    }
}
