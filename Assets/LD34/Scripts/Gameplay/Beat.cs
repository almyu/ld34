using UnityEngine;
using JamSuite.Events;

namespace LD34 {

    public class Beat : MonoBehaviour {

        public PulseEvent onPulse;
        public float offset = 6f;

        [System.Serializable]
        public struct BeatLayer {
            public float offset, bpm;
        }

        public BeatLayer[] layers;

        private void Awake() {
            foreach (var layer in layers)
                InvokeRepeating("SpawnBeat", layer.offset + offset, 60f / layer.bpm);
        }

        private void EmitBeat() {
            onPulse.Invoke(Time.timeSinceLevelLoad, 0f);
        }
    }
}
