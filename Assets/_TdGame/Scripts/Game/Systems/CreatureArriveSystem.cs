using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class CreatureArriveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Position> positionPool;
        EcsPool<DestroyMarker> destroyMarkerPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            positionPool = world.GetPool<Position>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
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
