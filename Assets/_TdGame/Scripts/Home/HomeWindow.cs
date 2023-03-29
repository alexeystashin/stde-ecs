using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Home
{
    public class HomeWindow : MonoBehaviour
    {
        [SerializeField] Button easyGameButton;
        [SerializeField] Button mediumGameButton;
        [SerializeField] Button hardGameButton;

        void Start()
        {
            easyGameButton.onClick.AddListener(OnEasyGameButtonTap);
            mediumGameButton.onClick.AddListener(OnMediumGameButtonTap);
            hardGameButton.onClick.AddListener(OnHardGameButtonTap);
        }

        void OnEasyGameButtonTap()
        {
            PlayerPrefs.SetInt("level", 0);
            SceneManager.LoadScene(1);
        }

        void OnMediumGameButtonTap()
        {
            PlayerPrefs.SetInt("level", 1);
            SceneManager.LoadScene(1);
        }

        void OnHardGameButtonTap()
        {
            PlayerPrefs.SetInt("level", 2);
            SceneManager.LoadScene(1);
        }
    }
}
