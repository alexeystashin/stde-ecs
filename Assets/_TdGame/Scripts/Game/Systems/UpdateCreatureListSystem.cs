using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class UpdateCreatureListSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Position> positionPool;

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
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Creature>().Inc<Position>().Exc<DestroyMarker>().End();

            for (var i = 0; i < gameState.creaturesByLine.Count; i++)
            {
                gameState.creaturesByLine[i].Clear();
            }

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                gameState.creaturesByLine[position.lineId].Add(entity);
            }
        }
    }
}
