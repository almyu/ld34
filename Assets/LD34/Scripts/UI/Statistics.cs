using UnityEngine;
using UnityEngine.UI;

namespace LD34.UI {

    public class Statistics : MonoBehaviour {

        public Score score;
        public Text goodsText, greatsText, perfectsText, accuracyText, maxComboText, scoreText;
        public GameObject[] stuffToDisable;

        private void OnEnable() {
            foreach (var obj in stuffToDisable)
                obj.SetActive(false);

            goodsText.text = score.goods.ToString();
            greatsText.text = score.greats.ToString();
            perfectsText.text = score.perfects.ToString();
            accuracyText.text = score.accuracy.ToString("p0");
            maxComboText.text = score.maxCombo.ToString();
            scoreText.text = score.score.ToString();
        }

        public void GoToMenu() {
            Application.LoadLevel(0);
        }
    }
}
