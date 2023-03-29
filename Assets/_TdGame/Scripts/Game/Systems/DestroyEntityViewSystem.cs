using Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class DestroyEntityViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<View> viewPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            viewPool = world.GetPool<View>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<DestroyMarker>().Inc<View>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                if (view.viewObject != null)
                {
                    var detachParticles = view.viewObject.GetComponent<DetachParticles>();
                    if (detachParticles != null)
                    {
                        detachParticles.Detach();
                    }

                    GameObject.Destroy(view.viewObject);
                }
                view.viewObject = null;
                viewPool.Del(entity);
            }
        }
    }
}
