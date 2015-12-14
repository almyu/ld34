using UnityEngine;
using System.Collections.Generic;

namespace LD34.UI {

    [RequireComponent(typeof(RectTransform))]
    public class TimelineUI : MonoBehaviour {

        public PulseBar pulseTemplate;
        public float pixelsPerSecond = 100f;

        private void Update() {
            GetComponent<RectTransform>().anchoredPosition += Vector2.left * pixelsPerSecond * Time.deltaTime;
        }

        public void SpawnPulseBar(Pulse pulse) {
            var bar = Instantiate(pulseTemplate);
            var xf = bar.GetComponent<RectTransform>();

            xf.anchoredPosition = Vector2.right * pixelsPerSecond * (pulse.actionTime + InputMatcher.halfMaxError);
            xf.sizeDelta = xf.sizeDelta.WithX(pixelsPerSecond * Mathf.Max(Timeline.minLength, pulse.length));
            xf.SetParent(transform, false);
            xf.gameObject.SetActive(true);

            pulse.listeners.Add(bar);
        }
    }
}
