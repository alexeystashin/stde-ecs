using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class DestroyEntitySystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                world.DelEntity(entity);
            }
        }
    }
}
