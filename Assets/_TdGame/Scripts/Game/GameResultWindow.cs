using Pump.Unity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TdGame
{
    public class GameResultWindow : MonoBehaviour
    {
        public RectTransform rootPanel;
        public TMP_Text titleText;
        public TMP_Text waveCounterText;
        public TMP_Text scoreText;
        public Button okButton;

        void Start ()
        {
            okButton.onClick.AddListener(OnOkButtonTap);

            StartCoroutine(ShowAnimation());
        }

        IEnumerator ShowAnimation()
        {
            rootPanel.localScale = Vector3.zero;

            yield return new WaitForSeconds(0.5f);

            var time = 0f;
            var timeTotal = 0.5f;
            while (time < timeTotal)
            {
                yield return new WaitForEndOfFrame();
                time = Mathf.Min(time + Time.deltaTime, timeTotal);
                rootPanel.localScale = Vector3.one * TweenUtils.EaseOutBounce(time / timeTotal);
            }
        }

        void OnOkButtonTap()
        {
            SceneManager.LoadScene(0);
        }
    }
}
