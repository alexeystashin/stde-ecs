using Leopotam.EcsLite;
using Pump.Unity;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class LifetimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Lifetime> lifetimePool;
        EcsPool<DestroyMarker> destroyMarketPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            lifetimePool = world.GetPool<Lifetime>();
            destroyMarketPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Lifetime>().End();

            foreach (int entity in filter)
            {
                ref var lifetime = ref lifetimePool.Get(entity);

                if (lifetime.lifetime > 0)
                    lifetime.lifetime = Mathf.Max(0, lifetime.lifetime - PumpTime.deltaTime);

                if (lifetime.lifetime <= 0)
                    destroyMarketPool.GetOrAdd(entity);
            }
        }
    }
}
