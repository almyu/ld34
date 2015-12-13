using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LD34 {

    public interface IInputMatchListener {
        void MissPulse();
        void ActivatePulse(Pulse pulse);
        void FinishPulse(Pulse pulse);
        void FailPulse(Pulse pulse);
    }

    public interface IPulseListener {
        void ActivatePulse();
        void FinishPulse();
        void FailPulse();
        void UpdatePulseProximity(float dt);
    }

    public class Pulse {
        public bool activated;
        public float actionTime, length;
        public List<IPulseListener> listeners;
    }

    public class InputMatcher : MonoBehaviour {

        public string inputButton = "Fire1";

        public float latency = 6f;
        public float maxError = 0.1f;

        [System.Serializable]
        public class PulseAddedEvent : UnityEvent<Pulse> {}

        public PulseAddedEvent onPulseAdded;

        public List<IInputMatchListener> listeners = new List<IInputMatchListener>();


        private Queue<Pulse> queue = new Queue<Pulse>();

        public void AddPulse(float length) {
            AddPulse(Time.timeSinceLevelLoad + latency, length);
        }

        public void AddPulse(float time, float length) {
            var pulse = new Pulse {
                activated = false,
                actionTime = time,
                length = length,
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
                foreach (var listener in listeners)
                    listener.MissPulse();
                return;
            }

            pulse.activated = true;
            pulse.actionTime += pulse.length;

            foreach (var listener in listeners)
                listener.ActivatePulse(pulse);

            foreach (var listener in pulse.listeners)
                listener.ActivatePulse();
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
                    listener.FailPulse(pulse);

                queue.Dequeue();
                return;
            }

            foreach (var listener in pulse.listeners)
                listener.FinishPulse();

            foreach (var listener in listeners)
                listener.FinishPulse(pulse);

            queue.Dequeue();
        }

        private void Update() {
            if (Input.GetButtonDown(inputButton)) StartInput();
            if (Input.GetButtonUp(inputButton)) EndInput();

            if (queue.Count != 0) {
                var pulse = queue.Peek();

                var prox = Time.timeSinceLevelLoad - pulse.actionTime;

                foreach (var listener in pulse.listeners)
                    listener.UpdatePulseProximity(prox);

                // Too late, no matter what stage
                if (Time.timeSinceLevelLoad - pulse.actionTime > maxError) {
                    foreach (var listener in pulse.listeners)
                        listener.FailPulse();

                    foreach (var listener in listeners)
                        listener.FailPulse(pulse);

                    queue.Dequeue();
                }
            }
        }
    }
}
