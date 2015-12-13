using UnityEngine;

namespace LD34 {

    public class Shaker : MonoBehaviour {

        public float maxDist = 0.05f;

        private Vector3 lastOffset;

        private void OnEnable() {
            lastOffset = Vector3.zero;
        }

        private void Update() {
            transform.position -= lastOffset;
            lastOffset = Random.insideUnitCircle * maxDist;
            transform.position += lastOffset;
        }

        private void OnDisable() {
            transform.position -= lastOffset;
        }
    }
}
