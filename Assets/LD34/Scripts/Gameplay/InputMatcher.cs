using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LD34 {

    public interface IPulseListener {
        void MissPulse();
        void ActivatePulse(float timing);
        void FinishPulse(float timing);
        void FailPulse();
        void UpdatePulseProximity(float dt);
    }

    public class Pulse {
        public bool activated;
        public float actionTime, length;
        public List<IPulseListener> listeners;
    }

    public class InputMatcher : MonoBehaviour {

        public static float maxError = 0.64f;
        public static float longPulse = 1f;

        public static float halfMaxError {
            get { return maxError * 0.5f; }
        }

        public string inputButton = "Fire1";

        [System.Serializable]
        public class PulseAddedEvent : UnityEvent<Pulse> {}

        public PulseAddedEvent onPulseAdded;

        public List<GameObject> listenerObjects = new List<GameObject>();

        private Queue<Pulse> queue = new Queue<Pulse>();
        private List<IPulseListener> listeners = new List<IPulseListener>();

        private void Awake() {
            foreach (var lob in listenerObjects) {
                var listener = lob.GetComponent<IPulseListener>();
                if (listener != null) listeners.Add(listener);
            }
        }

        public void AddPulse(float time, float length) {
            var pulse = new Pulse {
                activated = false,
                actionTime = time,
                length = length >= longPulse ? length : 0f,
                listeners = new List<IPulseListener>()
            };
            queue.Enqueue(pulse);
            onPulseAdded.Invoke(pulse);
        }

        public void StartInput() {
            if (queue.Count == 0) {
                foreach (var listener in listeners)
                    listener.MissPulse();

                return;
            }

            var pulse = queue.Peek();

            // Too early, don't discard
            if (Time.timeSinceLevelLoad < pulse.actionTime) {
                foreach (var listener in pulse.listeners)
                    listener.MissPulse();

                foreach (var listener in listeners)
                    listener.MissPulse();

                return;
            }

            var timing = CalcTiming(pulse.actionTime);

            pulse.activated = true;
            pulse.actionTime += pulse.length;

            foreach (var listener in listeners)
                listener.ActivatePulse(timing);

            foreach (var listener in pulse.listeners)
                listener.ActivatePulse(timing);

            if (pulse.length < longPulse) {
                foreach (var listener in listeners)
                    listener.FinishPulse(timing);

                foreach (var listener in pulse.listeners)
                    listener.FinishPulse(timing);

                queue.Dequeue();
            }
        }

        public void EndInput() {
            if (queue.Count == 0) return;

            var pulse = queue.Peek();

            // Held since miss or fail, no worries
            if (!pulse.activated) return;

            // Too early, discard
            if (Time.timeSinceLevelLoad < pulse.actionTime) {
                foreach (var listener in pulse.listeners)
                    listener.FailPulse();

                foreach (var listener in listeners)
                    listener.FailPulse();

                queue.Dequeue();
                return;
            }

            var timing = CalcTiming(pulse.actionTime);

            foreach (var listener in pulse.listeners)
                listener.FinishPulse(timing);

            foreach (var listener in listeners)
                listener.FinishPulse(timing);

            queue.Dequeue();
        }

        private float CalcTiming(float actionTime, bool abs = true) {
            var dt = Time.timeSinceLevelLoad - actionTime;
            var timing = dt - halfMaxError;
            return abs ? Mathf.Abs(timing) : timing;
        }

        private void Update() {
            if (Input.GetButtonDown(inputButton)) StartInput();
            if (Input.GetButtonUp(inputButton)) EndInput();

            if (queue.Count != 0) {
                var pulse = queue.Peek();

                var prox = Time.timeSinceLevelLoad - pulse.actionTime;

                foreach (var listener in pulse.listeners)
                    listener.UpdatePulseProximity(prox);

                foreach (var listener in listeners)
                    listener.UpdatePulseProximity(prox);

                // Too late, no matter what stage
                if (Time.timeSinceLevelLoad - pulse.actionTime > maxError) {
                    foreach (var listener in pulse.listeners)
                        listener.FailPulse();

                    foreach (var listener in listeners)
                        listener.FailPulse();

                    queue.Dequeue();
                }
            }
        }
    }
}
