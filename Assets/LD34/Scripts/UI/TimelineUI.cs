using UnityEngine;
using UnityEngine.UI;

namespace LD34.UI {

    [RequireComponent(typeof(RectTransform))]
    public class TimelineUI : MonoBehaviour {

        public RectTransform impulseTemplate;
        public Timeline timeline;
        public float pixelsPerSecond = 100f;

        private void Awake() {
            foreach (var imp in timeline.impulses)
                SpawnImpulse(imp);
        }

        private RectTransform SpawnImpulse(Timeline.Impulse imp) {
            var xf = Instantiate(impulseTemplate);
            xf.anchoredPosition = xf.anchoredPosition.WithX(imp.position * pixelsPerSecond);
            xf.sizeDelta = xf.sizeDelta.WithX(imp.clampedLength * pixelsPerSecond);
            xf.SetParent(transform, false);
            xf.gameObject.SetActive(true);
            return xf;
        }
    }
}
