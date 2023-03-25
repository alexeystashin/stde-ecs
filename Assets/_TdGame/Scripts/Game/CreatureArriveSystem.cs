using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class CreatureArriveSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var destroyMarkerPool = world.GetPool<DestroyMarker>();

            var filter = world.Filter<Creature>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                if (position.z <= MagicNumbersGame.creatureArriveZ)
                {
                    destroyMarkerPool.Add(entity);
                }
            }
        }
    }
}
