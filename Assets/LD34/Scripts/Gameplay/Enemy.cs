using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace LD34 {

    public class Enemy : MonoBehaviour, IPulseListener {

        public UnityEvent onHit, onDeath;

        public float speed = 3f;
        public float speedAfterHit = 1f;
        public float afterlife = 5f;

        private bool charge;
        private Vector2 velocity;

        private Hero.Hand hand;
        private bool wasActivated, nextHitIsLethal;

        public void ActivatePulse(float timing, Hero.Hand hand) {
            this.hand = hand;
            wasActivated = true;
            Hero.instance.OpenFire(transform, hand);
        }

        public void FinishPulse(float timing) {
            Hero.instance.CeaseFire(hand);
            nextHitIsLethal = true;
        }

        public void FailPulse() {
            if (wasActivated) Hero.instance.CeaseFire(hand);
            charge = true;
        }

        public void MissPulse() {}

        public void UpdatePulseProximity(float dt) {}

        public void ProjectileHit() {
            if (nextHitIsLethal) {
                BlowUp();
                return;
            }
            speed = speedAfterHit;
            onHit.Invoke();
        }

        public void BlowUp() {
            onDeath.Invoke();
            Destroy(gameObject, afterlife);
        }

        private void Update() {
            if (!charge) {
                var pos = transform.position;
                transform.position += Vector3.left * speed * Time.deltaTime;
                velocity = (transform.position - pos) / Time.deltaTime;
            }
            else {
                velocity.x -= 20f * Time.deltaTime;
                transform.position += (Vector3)(velocity * Time.deltaTime);
            }
        }
    }
}
