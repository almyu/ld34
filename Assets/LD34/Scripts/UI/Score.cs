using UnityEngine;
using UnityEngine.UI;

namespace LD34.UI {

    public class Score : MonoBehaviour {

        public Text comboText, scoreText;

        public int maxCombo;

        public int score {
            get { return _score; }
            set {
                _score = value;
                scoreText.text = value.ToString();
            }
        }

        public int perfects {
            get { return _perfects; }
            set {
                score += (value - _perfects) * combo * 10;
                _perfects = value;
            }
        }

        public int greats {
            get { return _greats; }
            set {
                score += (value - _greats) * combo * 8;
                _greats = value;
            }
        }

        public int goods {
            get { return _goods; }
            set {
                score += (value - _goods) * combo * 5;
                _goods = value;
            }
        }

        public int fails {
            get { return _fails; }
            set {
                _fails = value;
            }
        }

        public int combo {
            get { return _combo; }
            set {
                maxCombo = Mathf.Max(maxCombo, value);
                _combo = value;
                comboText.text = value.ToString();
            }
        }

        public float accuracy {
            get {
                var oks = _perfects + _greats + _goods;
                return (float) oks / (oks + _fails);
            }
        }

        private int _score, _combo, _perfects, _greats, _goods, _fails;

        public void Add(string fieldName) {
            if (fieldName == "perfects") ++perfects;
            else if (fieldName == "greats") ++greats;
            else if (fieldName == "goods") ++goods;
        }

        private void Awake() {
            combo = 0;
            score = 0;

            Invoke("ShowStatistics", Menu.track.length + 3f);
        }

        public GameObject statistics;

        public void ShowStatistics() {
            statistics.SetActive(true);
        }
    }
}
