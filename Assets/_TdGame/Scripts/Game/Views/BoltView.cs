using UnityEngine;

namespace TdGame
{
    public class BoltView : EntityView
    {
        [SerializeField] float disposeDelay;

        [SerializeField] GameObject bolt;
        [SerializeField] TrailRenderer trail;
        [SerializeField] ParticleSystem particles;

        float smoothDisposeTime;

        void Update()
        {
            if (smoothDisposeTime > 0)
            {
                smoothDisposeTime -= Time.deltaTime;
                if (smoothDisposeTime <= 0)
                    Dispose();
            }
        }

        public override void OnDespawned()
        {
            base.OnDespawned();

            smoothDisposeTime = 0;

            if (bolt)
                bolt.SetActive(true);
            if (trail != null)
                trail.Clear();
            if (particles != null)
            {
                particles.Clear();
                particles.Play();
            }
        }

        public override void SmoothDispose()
        {
            if (disposeDelay > 0)
            {
                smoothDisposeTime = disposeDelay;

                if (bolt != null)
                    bolt.SetActive(false);

                if (particles != null)
                    particles.Stop();
            }
            else
                Dispose();
        }
    }
}
