﻿using UnityEngine;

namespace LD34.UI {

    [RequireComponent(typeof(Blinker))]
    public class PulseBar : MonoBehaviour, IPulseListener {

        public Color activeColor;
        public Blinker finishBlinker, failBlinker;

        public void ActivatePulse(float timing, Hero.Hand hand) {
            GetComponent<CanvasRenderer>().SetColor(activeColor);
        }

        public void FinishPulse(float timing) {
            finishBlinker.Blink();
            Destroy(gameObject, finishBlinker.duration);
        }

        public void FailPulse() {
            failBlinker.Blink();
            Destroy(gameObject, failBlinker.duration);
        }

        public void MissPulse() {}

        public void UpdatePulseProximity(float dt) {}
    }
}
