using Leopotam.EcsLite;

namespace TdGame
{
    sealed class UpdateEntityViewPositionSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<View> viewPool;
        EcsPool<Position> positionPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            viewPool = world.GetPool<View>();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<View>().Inc<Position>().Exc<AnimationMarker>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                view.viewObject.transform.position = GameUtils.PositionToVector3(position.lineId, position.x, position.z);
            }
        }
    }
}
