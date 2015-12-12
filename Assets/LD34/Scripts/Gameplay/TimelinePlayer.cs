using UnityEngine;
using UnityEngine.Events;
using JamSuite.Events;

namespace LD34 {

    public class TimelinePlayer : MonoBehaviour {

        public Timeline timeline;
        public float time;
        public int pulseIndex;
        public bool pulseStarted;

        public FloatEvent onPulse;
        public UnityEvent onPulseStart, onPulseEnd, onTimelineEnd;

        public Timeline.Pulse pulse {
            get { return timeline.pulses[pulseIndex]; }
        }

        private void Update() {
            time += Time.deltaTime;

            if (pulse.position <= time && !pulseStarted) {
                pulseStarted = true;
                onPulseStart.Invoke();
                onPulse.Invoke(pulse.length);
            }

            if (pulse.position + pulse.clampedLength <= time) {
                onPulseEnd.Invoke();
                pulseStarted = false;
                NextPulse();
            }
        }

        public void NextPulse() {
            if (pulseIndex + 1 < timeline.pulses.Length)
                ++pulseIndex;
            else {
                onTimelineEnd.Invoke();
                pulseIndex = 0;
                time = 0f;
            }
        }
    }
}
