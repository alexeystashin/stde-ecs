using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TdGame
{
    public class GameResultWindow : MonoBehaviour
    {
        public TMP_Text titleText;
        public TMP_Text waveCounterText;
        public TMP_Text scoreText;
        public Button okButton;

        void Start ()
        {
            okButton.onClick.AddListener(OnOkButtonTap);
        }

        void OnOkButtonTap()
        {
            SceneManager.LoadScene(0);
        }
    }
}
