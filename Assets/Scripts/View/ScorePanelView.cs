using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ScorePanelView : MonoBehaviour
    {
        [SerializeField] private Text totalScoreText;
        [SerializeField] private Text currentScoreText;

        public void SetTotalScore(int value)
        {
            totalScoreText.text = value.ToString();
        }

        public void SetCurrentScore(int value)
        {
            currentScoreText.text = value.ToString();
        }
    }
}