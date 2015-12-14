using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace LD34 {

    public class TrackBeats : ScriptableObject {

        public float bpm = 120;
        public int[] beats;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Beats", priority = 220)]
        public static void Create() {
            AssetUtility.CreateAssetInSelectedDirectory(CreateInstance<TrackBeats>(), "Timeline");
        }

        public static TrackBeats CreateNextToTrack(AudioClip track) {
            var path = Path.ChangeExtension(AssetDatabase.GetAssetPath(track), "asset");
            var inst = CreateInstance<TrackBeats>();
            AssetDatabase.CreateAsset(inst, path);
            return inst;
        }

        public static TrackBeats LoadNextToTrack(AudioClip track) {
            var path = Path.ChangeExtension(AssetDatabase.GetAssetPath(track), "asset");
            return AssetDatabase.LoadAssetAtPath<TrackBeats>(path);
        }
#endif
    }
}
