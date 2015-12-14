using UnityEngine;
using UnityEngine.Events;

namespace LD34 {

    [System.Serializable]
    public class PulseEvent : UnityEvent<float, float> {}

    public class TimelinePlayer : MonoBehaviour {

        public Timeline timeline;
        public int pulseIndex;
        public float offset = 6f;

        [HideInInspector]
        public float time;

        public PulseEvent onPulse;
        public UnityEvent onTimelineEnd;

        private void Update() {
            if (pulseIndex >= timeline.pulses.Length) {
                onTimelineEnd.Invoke();
                enabled = false;
                return;
            }

            var pulse = timeline.pulses[pulseIndex];
            time += Time.deltaTime;

            if (pulse.position <= time) {
                var latency = time - pulse.position;
                onPulse.Invoke(Time.timeSinceLevelLoad - latency + offset, pulse.length);
                ++pulseIndex;
            }
        }
    }
}
