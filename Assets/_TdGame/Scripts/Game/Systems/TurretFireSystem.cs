using Leopotam.EcsLite;

namespace TdGame
{
    sealed class TurretFireSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<Position> positionPool;
        EcsPool<Cooldown> cooldownPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            turretPool = world.GetPool<Turret>();
            positionPool = world.GetPool<Position>();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Turret>().Inc<TurretFireTrigger>().Exc<AnimationMarker>().End();

            foreach (int entity in filter)
            {
                ref var turret = ref turretPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);

                if (cooldown.cooldown <= 0 && context.creaturesByLine[position.lineId].Count > 0)
                {
                    cooldown.cooldown = turret.template.attackCooldown;

                    var boltTemplate = context.staticGameData.bolts[turret.template.attackBoltId];
                    context.objectBuilder.CreateBolt(boltTemplate, position.lineId, position.z);
                }
            }
        }
    }
}
