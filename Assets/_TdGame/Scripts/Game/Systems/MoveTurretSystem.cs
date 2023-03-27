using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class MoveTurretSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<Position> positionPool;
        EcsPool<TurretMotion> turretMotionPool;
        EcsPool<AnimationMarker> animationMarkerPool;
        EcsPool<View> viewPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            turretPool = world.GetPool<Turret>();
            positionPool = world.GetPool<Position>();
            turretMotionPool = world.GetPool<TurretMotion>();
            animationMarkerPool = world.GetPool<AnimationMarker>();
            viewPool = world.GetPool<View>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<TurretMotion>().End();

            foreach (int entity in filter)
            {
                ref var turret = ref turretPool.Get(entity);
                ref var turretMotion = ref turretMotionPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                turretMotion.time = Mathf.Min(turretMotion.time + Time.deltaTime, turretMotion.timeTotal);

                ref var view = ref viewPool.Get(entity);
                var t = turretMotion.time / turretMotion.timeTotal;
                var currentLine = Mathf.Lerp(turretMotion.fromLineId, turretMotion.toLineId, t);
                var currentRow = Mathf.Lerp(turretMotion.fromRowId, turretMotion.toRowId, t);
                view.viewObject.transform.position = GameUtils.CellToVector3(currentLine, currentRow);

                // todo: move to separate system
                if (turretMotion.time >= turretMotion.timeTotal)
                {
                    //Debug.Log($"MoveTurret finished {entity} from {turretMotion.fromLineId}:{turretMotion.fromRowId} to {turretMotion.toLineId}:{turretMotion.toRowId}");
                    turret.lineId = turretMotion.toLineId;
                    turret.rowId = turretMotion.toRowId;

                    position.lineId = turretMotion.toLineId;
                    position.z = GameUtils.RowToZ(turret.rowId);

                    turretMotionPool.Del(entity);

                    ref var animationMarker = ref animationMarkerPool.Get(entity);
                    animationMarker.usageCounter--;

                    // todo: move to separate system
                    if (animationMarker.usageCounter <= 0)
                    {
                        animationMarkerPool.Del(entity);
                    }
                }
            }
        }
    }
}
