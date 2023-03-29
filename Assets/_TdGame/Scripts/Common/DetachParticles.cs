using UnityEngine;

namespace Common
{
    public class DetachParticles : MonoBehaviour
    {
        [SerializeField] ParticleSystem particleSystem;
        [SerializeField] float delay;

        public void Detach()
        {
            if (particleSystem != null)
            {
                particleSystem.loop = false;
                particleSystem.emissionRate = 0;

                var delayedDestroy = particleSystem.gameObject.AddComponent<DelayedDestroy>();
                delayedDestroy.time = delay;

                particleSystem.gameObject.transform.parent = null;
                particleSystem = null;
            }
        }
    }
}