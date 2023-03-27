using Leopotam.EcsLite;

namespace TdGame
{
    sealed class RemoveTurretFireTriggerSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<TurretFireTrigger> turretFireTriggerPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            turretFireTriggerPool = world.GetPool<TurretFireTrigger>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<TurretFireTrigger>().End();

            foreach (int entity in filter)
            {
                turretFireTriggerPool.Del(entity);
            }
        }
    }
}
