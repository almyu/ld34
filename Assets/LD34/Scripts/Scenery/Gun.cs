using UnityEngine;

namespace LD34 {

    public class Gun : MonoBehaviour {

        public Transform tip;
        public Projectile projectilePrefab;
        public float shotInterval = 0.25f;

        private Enemy target;
        private float shotTimer;

        public void OpenFire(Transform enemy) {
            target = enemy.GetComponent<Enemy>();
            if (!target) return;

            shotTimer = 0f;
            Update();
            enabled = true;
        }

        public void CeaseFire() {
            target = null;
            enabled = false;
        }

        private void Update() {
            shotTimer -= Time.deltaTime;

            if (shotTimer <= 0f) {
                shotTimer = shotInterval;
                SpawnProjectile();
            }
        }

        private void SpawnProjectile() {
            var proj = (Projectile) Instantiate(projectilePrefab, tip.position, tip.rotation);
            proj.target = target;
            proj.timeLeft = TimeSync.CeilTimerToBeat(proj.timeLeft);
        }
    }
}
