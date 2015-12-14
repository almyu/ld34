using UnityEngine;

namespace LD34 {

    public class LaserGun : MonoBehaviour {

        public Transform tip;
        public LaserBeam beamPrefab;
        public GameObject impactPrefab;
        public float impactLifetime = 2f;

        public void Fire(Vector3 point) {
            var dir = (point - tip.position).WithZ(0);
            transform.right = dir.normalized;
            SpawnBeam(dir.magnitude);
            SpawnImpact(point);
        }

        public void SpawnBeam(float length) {
            var beam = (LaserBeam) Instantiate(beamPrefab, tip.position, tip.rotation);
            beam.SetLength(length);
        }

        public void SpawnImpact(Vector3 point) {
            var impact = (GameObject) Instantiate(impactPrefab, point, Quaternion.identity);
            Destroy(impact, impactLifetime);
        }
    }
}
