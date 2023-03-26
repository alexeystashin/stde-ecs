using Leopotam.EcsLite;

namespace TdGame
{
    sealed class UpdateCreatureListSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Position> positionPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Creature>().Inc<Position>().Exc<DestroyMarker>().End();

            for (var i = 0; i < context.creaturesByLine.Count; i++)
            {
                context.creaturesByLine[i].Clear();
            }

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                context.creaturesByLine[position.lineId].Add(entity);
            }
        }
    }
}
