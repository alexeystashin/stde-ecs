using Leopotam.EcsLite;

namespace TdGame
{
    sealed class TriggerAreaByCooldownSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Area> areaPool;
        EcsPool<AreaTrigger> areaTriggerPool;
        EcsPool<Position> positionPool;
        EcsPool<Cooldown> cooldownPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            areaPool = world.GetPool<Area>();
            areaTriggerPool = world.GetPool<AreaTrigger>();
            positionPool = world.GetPool<Position>();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Area>().Inc<Cooldown>().Exc<AreaTrigger>().End();

            foreach (int entity in filter)
            {
                ref var area = ref areaPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);

                //if (!area.template.autoAttack)
                //    continue;

                if (cooldown.cooldown <= 0)
                {
                    areaTriggerPool.Add(entity);
                }
            }
        }
    }
}
