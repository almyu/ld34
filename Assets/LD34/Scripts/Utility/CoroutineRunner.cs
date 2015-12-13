using UnityEngine;
using System.Collections;

namespace LD34 {

    public static class CoroutineRunner {

        private static MonoBehaviour _instance;

        public static MonoBehaviour instance {
            get {
                if (!_instance) {
                    var obj = new GameObject("_CoroutineRunner");
                    _instance = obj.AddComponent<MonoBehaviour>();
                }
                return _instance;
            }
        }

        public static Coroutine Start(IEnumerator routine) {
            return instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine routine) {
            instance.StopCoroutine(routine);
        }

        public static void StopAll() {
            instance.StopAllCoroutines();
        }
    }
}
