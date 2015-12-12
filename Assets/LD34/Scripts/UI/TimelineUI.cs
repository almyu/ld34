using UnityEngine;
using UnityEngine.UI;

namespace LD34.UI {

    [RequireComponent(typeof(RectTransform))]
    public class TimelineUI : MonoBehaviour {

        public RectTransform pulseTemplate;
        public Timeline timeline;
        public float pixelsPerSecond = 100f;

        private RectTransform currentPulse;

        private void Update() {
            AnimatePulses();
            if (currentPulse) GrowPulse();
        }

        public void StartPulse() {
            currentPulse = Instantiate(pulseTemplate);
            currentPulse.anchoredPosition = Vector2.zero;
            currentPulse.SetParent(transform, false);
            currentPulse.gameObject.SetActive(true);
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
    }
}
