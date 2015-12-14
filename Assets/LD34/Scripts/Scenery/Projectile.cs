using UnityEngine;

namespace LD34 {

    public class Projectile : MonoBehaviour {

        public float timeLeft = 1f;

        [HideInInspector]
        public Enemy target;

        private void Update() {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0f || !target) {
                BlowUp();
                return;
            }

            var toTarget = target.transform.position - transform.position;
            var dist = toTarget.magnitude;
            var speed = dist / timeLeft;

            if (!Mathf.Approximately(dist, 0f))
                transform.up = toTarget / dist;

            transform.position += transform.up * speed * Time.deltaTime;
        }

        public void BlowUp() {
            if (target) target.ProjectileHit();
            Destroy(gameObject);
        }
    }
}
