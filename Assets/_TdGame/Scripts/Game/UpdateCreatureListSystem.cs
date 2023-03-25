using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class UpdateCreatureListSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var context = systems.GetShared<GameContext>();
            var positionPool = world.GetPool<Position>();

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
