using UnityEngine;

namespace LD34 {

    [RequireComponent(typeof(AudioSource))]
    public class TimeSync : MonoBehaviour {

        public static float time {
            get { return byTrack ? source.time : Time.timeSinceLevelLoad; }
        }

        public static float CeilTimerToBeat(float minTimeLeft) {
            return CeilToBeat(time + minTimeLeft) - time;
        }

        public static float CeilToBeat(float time) {
            var beatInterval = 60f / Menu.beats.bpm;
            return Mathf.Ceil(time / beatInterval) * beatInterval;
        }

        public static bool byTrack;
        public static AudioSource source;

        public enum Mode {
            ByTrack,
            ByLevel
        }
        public Mode mode;

        private void Awake() {
            source = GetComponent<AudioSource>();
            byTrack = mode == Mode.ByTrack;
        }
    }
}
