using UnityEngine;

namespace LD34 {

    public static class Scenery {

        public static GameObject FindPlayer() {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) Debug.LogError("No game object tagged Player found");
            return player;
        }
    }
}
