using UnityEngine;
using UnityEngine.UI;

namespace LD34 {

    public class SpectralTint : MonoBehaviour {

        public AudioSource source;
        public int numSamples = 64;
        public int channel = 0;
        public FFTWindow window;

        [System.Serializable]
        public class Target {
            public int sample;
            public float factor;
            public CanvasRenderer renderer;
        }

        public Target[] targets;

        public float[] samples;

        private void Awake() {
            samples = new float[numSamples];
        }

        private void Update() {
            source.GetSpectrumData(samples, channel, window);

            foreach (var target in targets)
                target.renderer.SetAlpha(samples[target.sample] * target.factor);
        }
    }
}
