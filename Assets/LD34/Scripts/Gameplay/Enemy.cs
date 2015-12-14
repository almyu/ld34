using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace LD34 {

    public class Enemy : MonoBehaviour, IPulseListener {

        public UnityEvent onHit, onDeath;

        public float speed = 3f;
        public float speedAfterHit = 1f;
        public float deathDelay = 1f;

        private bool charge;
        private Vector2 velocity;

        public void ActivatePulse(float timing, Hero.Hand hand) {
            speed = speedAfterHit;
            Hero.instance.AimAt(transform.position, hand);
        }

        public void FinishPulse(float timing) {
            onHit.Invoke();
            StartCoroutine(DoScheduleDeath());
        }

        public void FailPulse() {
            charge = true;
        }

        public void MissPulse() {}

        public void UpdatePulseProximity(float dt) {}

        private IEnumerator DoScheduleDeath() {
            yield return new WaitForSeconds(deathDelay);
            onDeath.Invoke();
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
