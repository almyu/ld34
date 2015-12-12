using UnityEngine;
using System.Collections;

namespace LD34 {

    public class Enemy : MonoBehaviour, IPulseListener {

        public Vector3 destiantion;
        public float speed = 10f;

        public void ActivatePulse() { }
        public void FinishPulse() { }
        public void FailPulse() { }

        //private 

        private void Update() {
            transform.position = Vector3.MoveTowards(transform.position, destiantion, speed * Time.deltaTime);
        }
    }
}
