using Leopotam.EcsLite;

namespace TdGame
{
    sealed class RemoveBoltTriggerSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<BoltTrigger> boltTriggerPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            boltTriggerPool = world.GetPool<BoltTrigger>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<BoltTrigger>().End();

            foreach (int entity in filter)
            {
                boltTriggerPool.Del(entity);
            }
        }
    }
}
