using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class TurretFireSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<Position> positionPool;
        EcsPool<Cooldown> cooldownPool;

        StaticGameData staticGameData;
        GameObjectBuilder objectBuilder;

        [Inject]
        void Construct(StaticGameData staticGameData, GameObjectBuilder objectBuilder)
        {
            this.staticGameData = staticGameData;
            this.objectBuilder = objectBuilder;
        }

        public void Init(IEcsSystems systems)
        {
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

                if (cooldown.cooldown <= 0)
                {
                    cooldown.cooldown = turret.template.attackCooldown;

                    var boltTemplate = staticGameData.bolts[turret.template.attackBoltId];
                    objectBuilder.CreateBolt(boltTemplate, position.lineId, position.z);
                }
            }
        }
    }
}
