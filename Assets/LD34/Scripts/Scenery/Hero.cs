using UnityEngine;

namespace LD34 {

    public class Hero : MonoSingleton<Hero> {

        public enum Hand {
            Left,
            Right
        }

        public Transform leftArm, leftHand, rightArm, rightHand;

        public GameObject[] weapons;

        private Transform leftTarget, rightTarget;


        public Transform GetArm(Hand hand) {
            return hand == Hand.Left ? leftArm : rightArm;
        }

        public Transform GetHand(Hand hand) {
            return hand == Hand.Left ? leftHand : rightHand;
        }
        
        public void OpenFire(Transform target, Hand hand) {
            if (hand == Hand.Left) leftTarget = target;
            else rightTarget = target;

            var arm = GetArm(hand);
            var dir = (target.position - arm.position).WithZ(0).normalized;

            GetArm(hand).right = dir;
            GetHand(hand).BroadcastMessage("OpenFire", target, SendMessageOptions.DontRequireReceiver);
        }

        public void CeaseFire(Hand hand) {
            if (hand == Hand.Left) leftTarget = null;
            else rightTarget = null;

            GetHand(hand).BroadcastMessage("CeaseFire", SendMessageOptions.DontRequireReceiver);
        }

        public void SwitchWeapon() {
            var hand = Random.value < 0.5f ? Hand.Left : Hand.Right;
            var wpn = weapons[Random.Range(0, weapons.Length)];

            SetWeapon(hand, wpn);
        }

        public void SetWeapon(Hand hand, GameObject weapon) {
            if (hand == Hand.Left && leftTarget) return;
            if (hand == Hand.Right && rightTarget) return;

            var grip = GetHand(hand);

            foreach (Transform child in grip.transform)
                Destroy(child.gameObject);

            var wpn = Instantiate(weapon);
            wpn.transform.SetParent(grip, false);
        }
    }
}
