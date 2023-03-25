using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class DestroyEntitySystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world.Filter<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                world.DelEntity(entity);
            }
        }
    }
}
