using UnityEngine;

namespace Pump.Unity
{
    public class PumpTimeWatcher : MonoBehaviour
    {
        Animator animator;
        ParticleSystem particles;

        float lastTimeScale;

        void Awake()
        {
            animator = GetComponent<Animator>();
            particles = GetComponent<ParticleSystem>();
            UpdateTimeScale();
        }

        void OnEnable()
        {
            UpdateTimeScale();
        }

        void LateUpdate()
        {
            UpdateTimeScale();
        }

        void UpdateTimeScale()
        {
            if (lastTimeScale == PumpTime.timeScale)
                return;

            lastTimeScale = PumpTime.timeScale;

            ApplyTimeScale(lastTimeScale);
        }

        void ApplyTimeScale(float timeScale)
        {
            if (animator != null)
                animator.speed = timeScale;

            if (particles != null)
                particles.playbackSpeed = timeScale;
        }

        void OnDestroy()
        {
            ApplyTimeScale(1.0f);
            animator = null;
            particles = null;
        }
    }
}
