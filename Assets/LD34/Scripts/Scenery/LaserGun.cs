using UnityEngine;

namespace LD34 {

    public class LaserGun : MonoBehaviour {

        public Transform tip;
        public LaserBeam beamPrefab;
        public GameObject impactPrefab;

        private Enemy target;
        private LaserBeam beam;
        private GameObject impact;

        public void OpenFire(Transform enemy) {
            target = enemy.GetComponent<Enemy>();
            if (!target) return;

            target.ProjectileHit();
            target.speed = 0f;

            beam = (LaserBeam) Instantiate(beamPrefab, tip.position, tip.rotation);
            impact = (GameObject) Instantiate(impactPrefab, enemy.position, Quaternion.identity);
            Update();
            enabled = true;
        }

        public void CeaseFire() {
            if (!enabled) return;

            if (target) {
                target.ProjectileHit();
                target = null;
            }
            beam.FadeOut();
            Destroy(impact);
            enabled = false;
        }

        private void Update() {
            if (!target) return;

            var dir = (target.transform.position - tip.position).WithZ(0);
            transform.right = dir.normalized;
            beam.SetLength(dir.magnitude);
            impact.transform.position = target.transform.position;
        }
    }
}
