using Leopotam.EcsLite;
using Pump.Unity;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class MoveEntitySystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Motion> motionPool;
        EcsPool<Position> positionPool;
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
            motionPool = world.GetPool<Motion>();
            positionPool = world.GetPool<Position>();
            freezedPool = world.GetPool<Freezed>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Motion>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var motion = ref motionPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                var velocityZ = motion.velocityZ;

                if (freezedPool.Has(entity))
                    velocityZ *= 0.2f;

                position.z += velocityZ * PumpTime.deltaTime;
            }
        }
    }
}
