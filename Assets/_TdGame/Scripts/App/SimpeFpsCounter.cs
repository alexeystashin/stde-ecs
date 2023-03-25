using TMPro;
using UnityEngine;

namespace App
{
    public class SimpeFpsCounter : MonoBehaviour
    {
        [SerializeField] TMP_Text counterText;

        [SerializeField] [Range(0f, 1f)] float expSmoothingFactor = 0.9f;
        [SerializeField] float refreshFrequency = 0.4f;

        float timeSinceUpdate = 0f;
        float averageFps = 1f;

        void Update()
        {
            // Exponentially weighted moving average (EWMA)
            averageFps = expSmoothingFactor * averageFps + (1f - expSmoothingFactor) * 1f / Time.unscaledDeltaTime;

            if (timeSinceUpdate < refreshFrequency)
            {
                timeSinceUpdate += Time.deltaTime;
                return;
            }

            int fps = Mathf.RoundToInt(averageFps);
            counterText.text = fps.ToString();

            timeSinceUpdate = 0f;
        }
    }
}
