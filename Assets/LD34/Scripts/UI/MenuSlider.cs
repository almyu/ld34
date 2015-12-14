using UnityEngine;
using UnityEngine.UI;

namespace LD34.UI {

    [RequireComponent(typeof(Slider))]
    public class MenuSlider : MonoBehaviour {

        public enum Mode {
            Volume,
            Lag
        }

        public Mode mode;
        public Text valueText;
        public string valueFormat = "{0}";

        private void Awake() {
            GetComponent<Slider>().onValueChanged.AddListener(SetValue);
        }

        public void SetValue(float value) {
            valueText.text = string.Format(valueFormat, value);

            if (mode == Mode.Volume) AudioListener.volume = value;
            else TimelinePlayer.lag = value * 0.001f;
        }

    }
}
