using UnityEngine;
using System.Collections.Generic;

namespace LD34.UI {

    [RequireComponent(typeof(RectTransform))]
    public class TimelineUI : MonoBehaviour {

        public PulseBar pulseTemplate;
        public Timeline timeline;
        public float pixelsPerSecond = 100f;

        public InputMatcher matcher;
        public float travelTime = 2f;

        private void Update() {
            GetComponent<RectTransform>().anchoredPosition += Vector2.left * pixelsPerSecond * Time.deltaTime;
        }

        public void AddPulse(float length) {
            var pulse = Instantiate(pulseTemplate);
            var xf = pulse.GetComponent<RectTransform>();
            var time = Time.timeSinceLevelLoad + travelTime;

            xf.anchoredPosition = Vector2.right * pixelsPerSecond * time;
            xf.sizeDelta = xf.sizeDelta.WithX(pixelsPerSecond * Mathf.Max(Timeline.minLength, length));
            xf.SetParent(transform, false);
            xf.gameObject.SetActive(true);

            // HACK
            //matcher.AddPulse(time, length, pulse);
        }
    }
}
