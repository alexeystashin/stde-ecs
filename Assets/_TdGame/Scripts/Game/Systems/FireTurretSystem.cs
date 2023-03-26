using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class FireTurretSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<Position> positionPool;
        EcsPool<AnimationMarker> animationMarkerPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            turretPool = world.GetPool<Turret>();
            positionPool = world.GetPool<Position>();
            animationMarkerPool = world.GetPool<AnimationMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Turret>().Inc<Position>().Exc<AnimationMarker>().End();

            foreach (int entity in filter)
            {
                ref var turret = ref turretPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                if (turret.cooldown > 0)
                    turret.cooldown = Mathf.Max(0, turret.cooldown - Time.deltaTime);

                if (turret.cooldown <= 0 && !animationMarkerPool.Has(entity) && context.creaturesByLine[position.lineId].Count > 0)
                {
                    context.objectBuilder.SpawnBullet(position.lineId, position.z);
                    turret.cooldown = MagicNumbersGame.turretFireCooldown;
                }
            }
        }
    }
}
