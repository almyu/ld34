using UnityEngine;
using UnityEngine.Serialization;

namespace LD34 {

    public class Timeline : ScriptableObject {

        public const float minLength = 0.1f;

        [System.Serializable]
        public struct Pulse {
            public float position, length;

            public float clampedLength {
                get { return Mathf.Max(minLength, length); }
            }
        }

        [FormerlySerializedAs("impulses")]
        public Pulse[] pulses;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Timeline", priority = 220)]
        public static void Create() {
            AssetUtility.CreateAssetInSelectedDirectory(CreateInstance<Timeline>(), "Timeline");
        }
#endif
    }
}
