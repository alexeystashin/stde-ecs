using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class DestroyViewSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var viewPool = world.GetPool<View>();

            var filter = world.Filter<DestroyMarker>().Inc<View>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                GameObject.Destroy(view.viewObject);
                view.viewObject = null;
                viewPool.Del(entity);
            }
        }
    }
}
