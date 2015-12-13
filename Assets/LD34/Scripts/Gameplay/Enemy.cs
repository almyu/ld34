using UnityEngine;
using System.Collections;

namespace LD34 {

    public class Enemy : MonoBehaviour, IPulseListener {

        public enum Mode {
            Approach,
            Fall,
            Attack
        }

        [HideInInspector]
        public Mode mode;

        public Vector3 destiantion;
        public float speed = 10f;

        private Vector2 velocity;

        public void ActivatePulse() {}

        public void FinishPulse() {
            mode = Mode.Fall;
        }

        public void FailPulse() {
            mode = Mode.Attack;
        }

        public void UpdatePulseProximity(float dt) {}

        private void Update() {
            if (mode == Mode.Approach) {
                var pos = transform.position;
                //transform.position = Vector3.MoveTowards(pos, destiantion, speed * Time.deltaTime);
                transform.position += Vector3.left * speed * Time.deltaTime;
                velocity = (transform.position - pos) / Time.deltaTime;
            }
            if (mode == Mode.Fall) {
                velocity.y -= 10f * Time.deltaTime;
                transform.position += (Vector3)(velocity * Time.deltaTime);

                //if (transform.position.y < 100f) Destroy(gameObject);
            }
            if (mode == Mode.Attack) {
                velocity.x -= 20f * Time.deltaTime;
                transform.position += (Vector3)(velocity * Time.deltaTime);

                //if (transform.position.x < 100f) Destroy(gameObject);
            }
        }
    }
}
