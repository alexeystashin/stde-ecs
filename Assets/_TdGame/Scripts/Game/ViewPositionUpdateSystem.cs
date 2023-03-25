using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class ViewPositionUpdateSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var viewPool = world.GetPool<View>();
            var positionPool = world.GetPool<Position>();

            var filter = world.Filter<View>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                view.viewObject.transform.position = GameUtils.PositionToVector3(position.lineId, position.x, position.z);
            }
        }
    }
}
