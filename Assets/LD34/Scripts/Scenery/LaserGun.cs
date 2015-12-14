using UnityEngine;

namespace LD34 {

    public class LaserGun : MonoBehaviour {

        public Transform tip;
        public SpriteRenderer beam;

        public void SetTarget(Vector3 point) {
            var dir = (point - tip.position).WithZ(0);
            transform.right = dir.normalized;
            SetBeamLength(dir.magnitude);
        }

        public void SetBeamLength(float length) {
            beam.transform.localScale = new Vector3(length, 1, 1);
        }
    }
}
