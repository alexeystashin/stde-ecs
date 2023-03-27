using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class LifetimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Lifetime> lifetimePool;
        EcsPool<DestroyMarker> destroyMarketPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            lifetimePool = world.GetPool<Lifetime>();
            destroyMarketPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Lifetime>().End();

            foreach (int entity in filter)
            {
                ref var lifetime = ref lifetimePool.Get(entity);

                if (lifetime.lifetime > 0)
                    lifetime.lifetime = Mathf.Max(0, lifetime.lifetime - Time.deltaTime);

                if (lifetime.lifetime <= 0)
                    destroyMarketPool.GetOrAdd(entity);
            }
        }
    }
}
