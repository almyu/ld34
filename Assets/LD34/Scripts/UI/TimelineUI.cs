using UnityEngine;
using UnityEngine.UI;

namespace LD34.UI {

    [RequireComponent(typeof(RectTransform))]
    public class TimelineUI : MonoBehaviour {

        public RectTransform impulseTemplate;
        public float pixelsPerSecond = 100f;
        public Timeline timeline;

        private void Awake() {
            foreach (var impulse in timeline.impulses)
                SpawnImpulse(impulse);
        }

        private RectTransform SpawnImpulse(float time) {
            var xf = Instantiate(impulseTemplate);
            xf.anchoredPosition = xf.anchoredPosition.WithX(time * pixelsPerSecond);
            xf.SetParent(transform, false);
            xf.gameObject.SetActive(true);
            return xf;
        }
    }
}
