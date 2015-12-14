using UnityEngine;

namespace LD34 {

    public class Hero : MonoSingleton<Hero> {

        public enum Hand {
            Left,
            Right
        }

        public Transform leftArm, leftHand, rightArm, rightHand;

        public Transform GetArm(Hand hand) {
            return hand == Hand.Left ? leftArm : rightArm;
        }

        public Transform GetHand(Hand hand) {
            return hand == Hand.Left ? leftHand : rightHand;
        }
        
        public void AimAt(Vector3 point, Hand hand) {
            var arm = GetArm(hand);
            var dir = (point.WithZ(0) - arm.position.WithZ(0)).normalized;

            GetArm(hand).right = dir;
            GetHand(hand).BroadcastMessage("Fire", point, SendMessageOptions.DontRequireReceiver);
        }
    }
}
