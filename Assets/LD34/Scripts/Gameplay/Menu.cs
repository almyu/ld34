using UnityEngine;

namespace LD34 {

    public class Menu : MonoBehaviour {

        public static AudioClip track;
        public static TrackBeats beats;

        public AudioClip defaultTrack;
        public TrackBeats defaultBeats;

        private void Awake() {
            if (!track) track = defaultTrack;
            if (!beats) beats = defaultBeats;
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.Escape) && Application.loadedLevel != 0)
                Application.LoadLevel(0);
        }

        public void SetTrack(AudioClip track_) {
            track = track_;
        }

        public void SetBeats(TrackBeats beats_) {
            beats = beats_;
        }

        public void Load() {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
