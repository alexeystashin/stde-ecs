using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class BoltArriveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Position> positionPool;
        EcsPool<DestroyMarker> destroyMarkerPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            positionPool = world.GetPool<Position>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Bolt>().Inc<Position>().Exc<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                if (position.z >= MagicNumbersGame.bulletArriveZ)
                {
                    destroyMarkerPool.Add(entity);
                }
            }
        }
    }
}
