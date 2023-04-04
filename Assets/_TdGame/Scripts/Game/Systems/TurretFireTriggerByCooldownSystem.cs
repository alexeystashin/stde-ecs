using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class TurretFireTriggerByCooldownSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<TurretFireTrigger> turretFireTriggerPool;
        EcsPool<Position> positionPool;
        EcsPool<Cooldown> cooldownPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            turretPool = world.GetPool<Turret>();
            turretFireTriggerPool = world.GetPool<TurretFireTrigger>();
            positionPool = world.GetPool<Position>();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Turret>().Exc<TurretFireTrigger>().Exc<AnimationMarker>().End();

            foreach (int entity in filter)
            {
                ref var turret = ref turretPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);

                if (!turret.template.autoAttack)
                    continue;

                if (cooldown.cooldown <= 0 && gameState.creaturesByLine[position.lineId].Count > 0)
                {
                    turretFireTriggerPool.Add(entity);
                }
            }
        }
    }
}
