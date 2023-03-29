using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TdGame
{
    public class GameUi : MonoBehaviour
    {
        public TMP_Text waveCounterText;
        public TMP_Text waveTimeText;
        public TMP_Text scoreText;
        public Button menuButton;
        public RectTransform hudContiner;

        void Start()
        {
            menuButton.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            //var gameStartup = GameObject.FindAnyObjectByType<GameStartup>();
            //Destroy(gameStartup.gameObject);

            SceneManager.LoadScene(0);
        }
    }
}
