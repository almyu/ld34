using UnityEngine;

namespace LD34 {

    [RequireComponent(typeof(AudioSource))]
    public class TimeSync : MonoBehaviour {

        public static float time {
            get { return byTrack ? source.time : Time.timeSinceLevelLoad; }
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
