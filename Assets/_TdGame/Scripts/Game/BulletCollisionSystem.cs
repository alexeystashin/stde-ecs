using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class BulletCollisionSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var context = systems.GetShared<GameContext>();
            var positionPool = world.GetPool<Position>();
            var destroyMarkerPool = world.GetPool<DestroyMarker>();

            var filter = world.Filter<Bullet>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                var creaturesByLine = context.creaturesByLine[position.lineId];
                for (var i = 0; i < creaturesByLine.Count; i++)
                {
                    if (destroyMarkerPool.Has(creaturesByLine[i]))
                        continue;

                    ref var creaturePosition = ref positionPool.Get(creaturesByLine[i]);
                    if (position.z >= creaturePosition.z)
                    {
                        Debug.Log($"Destroy Bullet at line {position.lineId} {position.x}:{position.z}");
                        destroyMarkerPool.Add(entity);
                        break;
                    }
                }
            }
        }
    }
}
