using Pump.Unity;
using UnityEngine;

namespace TdGame
{
    public class GameTime : MonoBehaviour, ITime
    {
        public float time { get; private set; }
        public float timeScale { get; private set; } = 1f;
        public float deltaTime { get; private set; }
        public float fixedDeltaTime { get; private set; }

        void OnEnable()
        {
            PumpTime.SetCustomTime(this);
        }

        void OnDisable()
        {
            PumpTime.SetCustomTime(null);
        }

        public void SetTimeScale(float timeScale)
        {
            this.timeScale = timeScale;
        }

        void Update()
        {
            deltaTime = Time.deltaTime * timeScale;
            time += deltaTime;
        }
    }
}
