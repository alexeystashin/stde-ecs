using Leopotam.EcsLite;

namespace TdGame
{
    sealed class RemoveAreaTriggerSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<AreaTrigger> areaTriggerPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            areaTriggerPool = world.GetPool<AreaTrigger>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<AreaTrigger>().End();

            foreach (int entity in filter)
            {
                areaTriggerPool.Del(entity);
            }
        }
    }
}
