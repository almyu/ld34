using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LD34 {

    [System.Serializable]
    public class PulseEvent : UnityEvent<float, float> {}

    public class TimelinePlayer : MonoBehaviour {

        public static float lag = 0.05f;

        public int shortBeatEventIndex;
        public int longBeatEventIndex;
        public float lookahead = 6f;

        private int shortFlag, longFlag;
        private List<Timeline.Pulse> pulses = new List<Timeline.Pulse>();
        private int pulseIndex;

        public PulseEvent onPulse;
        public UnityEvent onTimelineEnd;

        private void Awake() {
            shortFlag = 1 << shortBeatEventIndex;
            longFlag = 1 << longBeatEventIndex;

            var beats = Menu.beats;
            var sampleLength = 30f / beats.bpm;

            var hadLong = false;
            var longStart = 0f;

            for (int i = 0; i < beats.beats.Length; ++i) {
                var beat = beats.beats[i];
                var position = i * sampleLength;

                var hasShort = (beat & shortFlag) != 0;
                var hasLong = (beat & longFlag) != 0;

                if (hadLong) {
                    if (hasLong) continue;

                    pulses.Add(new Timeline.Pulse { position = longStart, length = position - longStart });
                    hadLong = false;
                    continue;
                }
                if (hasLong) {
                    longStart = position;
                    hadLong = true;
                    continue;
                }
                if (hasShort) {
                    pulses.Add(new Timeline.Pulse { position = position, length = 0f });
                    continue;
                }
            }
        }

        private void Update() {
            if (pulseIndex >= pulses.Count) {
                onTimelineEnd.Invoke();
                enabled = false;
                return;
            }

            var pulse = pulses[pulseIndex];
            var time = TimeSync.time + lookahead;

            if (pulse.position <= time) {
                var latency = time - pulse.position;
                onPulse.Invoke(Time.timeSinceLevelLoad + lookahead - latency - lag, pulse.length);
                ++pulseIndex;
            }
        }
    }
}
