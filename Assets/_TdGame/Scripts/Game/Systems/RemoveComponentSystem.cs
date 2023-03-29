using Leopotam.EcsLite;

namespace TdGame
{
    sealed class RemoveComponentSystem<T> : IEcsInitSystem, IEcsRunSystem where T: struct
    {
        EcsWorld world;
        EcsPool<T> componentPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            componentPool = world.GetPool<T>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<T>().End();

            foreach (int entity in filter)
            {
                componentPool.Del(entity);
            }
        }
    }
}
