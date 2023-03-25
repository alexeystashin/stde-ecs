using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class BulletArriveSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var destroyMarkerPool = world.GetPool<DestroyMarker>();

            var filter = world.Filter<Bullet>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                if (position.z >= MagicNumbersGame.bulletArriveZ)
                {

                    Debug.Log($"Destroy Bullet at line {position.lineId} {position.x}:{position.z}");
                    destroyMarkerPool.Add(entity);
                }
            }
        }
    }
}
