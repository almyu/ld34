using UnityEngine;

namespace LD34 {

    [RequireComponent(typeof(AudioSource))]
    public class TrackPlayer : MonoBehaviour {

        private void Awake() {
            var source = GetComponent<AudioSource>();
            source.clip = Menu.track;
            source.Play();
        }
    }
}
