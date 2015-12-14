using UnityEngine;
using UnityEngine.Events;

namespace LD34 {

    public class TrackPlayer : MonoBehaviour {

        public AudioSource source;
        public TrackBeats beats;
        public float offset = 6f;

        public PulseEvent[] onPulse;

        private int position;

        private void Awake() {
            InvokeRepeating("Emit", 0f, 30f / beats.bpm);
        }

        private void Emit() {
            var beat = beats.beats[position++];

            for (int eventIndex = 0; eventIndex < onPulse.Length; ++eventIndex) {
                if ((beat & (1 << eventIndex)) == 0) continue;

                onPulse[eventIndex].Invoke(source.time + offset, 0f);
            }

            if (position >= beats.beats.Length)
                CancelInvoke();
        }
    }
}
