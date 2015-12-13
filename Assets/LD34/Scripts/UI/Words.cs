using UnityEngine;
using JamSuite.UI;
using System;

namespace LD34 {

    public class Words : MonoBehaviour, IPulseListener {

        [Serializable]
        public struct Preset {
            public FloatingText emitter;
            public string text;
            public float timing;
        }

        public Preset[] hitPresets;

        public FloatingText failEmitter;
        public string failText = "Fail";

        public FloatingText missEmitter;
        public string missText = "Miss";

        public float minLength = 0.1f;

        private float lastActivationTime;

        private void Awake() {
            Array.Sort(Array.ConvertAll(hitPresets, p => p.timing), hitPresets);
        }

        public Preset PickHitPreset(float timing) {
            for (int i = 0; i < hitPresets.Length; ++i)
                if (timing <= hitPresets[i].timing)
                    return hitPresets[i];

            return hitPresets[hitPresets.Length - 1];
        }

        public void EmitHit(float timing) {
            var preset = PickHitPreset(timing);
            Emit(preset.emitter, preset.text);
        }

        public void Emit(FloatingText emitter, string text) {
            emitter.Spawn(transform.position, 0).text = text;
        }

        public void ActivatePulse(float timing) {
            EmitHit(timing);
            lastActivationTime = Time.timeSinceLevelLoad;
        }

        public void FailPulse() {
            Emit(failEmitter, failText);
        }

        public void FinishPulse(float timing) {
            if (Time.timeSinceLevelLoad - lastActivationTime < minLength) return;
            EmitHit(timing);
        }

        public void MissPulse() {
            Emit(missEmitter, missText);
        }

        public void UpdatePulseProximity(float dt) {}
    }
}
