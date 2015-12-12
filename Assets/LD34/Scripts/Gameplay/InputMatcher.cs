using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LD34 {

    public interface IPulseListener {
        void ActivatePulse();
        void FinishPulse();
        void FailPulse();
    }

    public class InputMatcher : MonoBehaviour {

        public string inputButton = "Fire1";

        public float maxError = 0.1f;
        public UnityEvent onStartHit, onEndHit, onMiss, onFail;

        private class PulseState {
            public bool activated;
            public float actionTime, length;
            public IPulseListener listener;
        }

        private Queue<PulseState> queue = new Queue<PulseState>();

        public void AddPulse(float length) {
            AddPulse(Time.timeSinceLevelLoad, length, null);
        }

        public void AddPulse(float time, float length, IPulseListener callback) {
            queue.Enqueue(new PulseState {
                activated = false,
                actionTime = time,
                length = length,
                listener = callback
            });
        }

        public void StartInput() {
            if (queue.Count == 0) {
                onMiss.Invoke();
                return;
            }

            var pulse = queue.Peek();

            // Too early, don't discard
            if (Time.timeSinceLevelLoad < pulse.actionTime) {
                onMiss.Invoke();
                return;
            }

            pulse.activated = true;
            pulse.actionTime += pulse.length;

            if (pulse.listener != null)
                pulse.listener.ActivatePulse();

            onStartHit.Invoke();
        }

        public void EndInput() {
            if (queue.Count == 0) return;

            var pulse = queue.Peek();

            // Held since miss or fail, no worries
            if (!pulse.activated) return;

            // Too early, discard
            if (Time.timeSinceLevelLoad < pulse.actionTime) {
                if (pulse.listener != null)
                    pulse.listener.FailPulse();

                onFail.Invoke();
                queue.Dequeue();
                return;
            }

            if (pulse.listener != null)
                pulse.listener.FinishPulse();

            onEndHit.Invoke();
            queue.Dequeue();
        }

        private void Update() {
            if (Input.GetButtonDown(inputButton)) StartInput();
            if (Input.GetButtonUp(inputButton)) EndInput();

            if (queue.Count != 0) {
                var pulse = queue.Peek();

                // Too late, no matter what stage
                if (Time.timeSinceLevelLoad - pulse.actionTime > maxError) {
                    if (pulse.listener != null)
                        pulse.listener.FailPulse();

                    onFail.Invoke();
                    queue.Dequeue();
                }
            }
        }
    }
}
