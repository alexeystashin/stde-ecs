using Leopotam.EcsLite;
using Pump.Unity;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class FreezedTimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Freezed> freezedPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            freezedPool = world.GetPool<Freezed>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Freezed>().End();

            foreach (int entity in filter)
            {
                ref var freezed = ref freezedPool.Get(entity);

                if (freezed.time > 0)
                    freezed.time = Mathf.Max(0, freezed.time - PumpTime.deltaTime);

                if (freezed.time <= 0)
                    freezedPool.Del(entity);
            }
        }
    }
}
