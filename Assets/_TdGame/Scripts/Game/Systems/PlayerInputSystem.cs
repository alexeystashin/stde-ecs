using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<TurretMotion> turretMotionPool;
        EcsPool<AnimationMarker> animationMarkerPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            turretPool = world.GetPool<Turret>();
            turretMotionPool = world.GetPool<TurretMotion>();
            animationMarkerPool = world.GetPool<AnimationMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var touch in context.gameInput.touches)
            {
                if (touch.isProcessed)
                    continue;

                if (Vector3.Distance(touch.currentTouchPos, touch.startTouchPos) >= MagicNumbersGame.dragThresold * Screen.height)
                {
                    touch.isProcessed = true;

                    var firstTurretEntity = GetTurretEntityUnderTouch(touch.startTouchPos);

                    if (firstTurretEntity == 0)
                        continue;

                    var delta = touch.currentTouchPos - touch.startTouchPos;
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        if (delta.x > 0)
                        {
                            Debug.Log("Swipe Right");

                            ref var firstTurret = ref turretPool.Get(firstTurretEntity);
                            if (firstTurret.lineId < MagicNumbersGame.lineCount - 1)
                            {
                                var secondTurretEntity = GetTurret(firstTurret.lineId + 1, firstTurret.rowId);

                                if (!turretMotionPool.Has(firstTurretEntity) && !turretMotionPool.Has(secondTurretEntity))
                                {
                                    MoveTurret(firstTurretEntity, firstTurret.lineId, firstTurret.rowId, firstTurret.lineId + 1, firstTurret.rowId);

                                    if (secondTurretEntity != 0)
                                        MoveTurret(secondTurretEntity, firstTurret.lineId + 1, firstTurret.rowId, firstTurret.lineId, firstTurret.rowId);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Swipe Left");

                            ref var firstTurret = ref turretPool.Get(firstTurretEntity);
                            if (firstTurret.lineId > 0)
                            {
                                var secondTurretEntity = GetTurret(firstTurret.lineId - 1, firstTurret.rowId);

                                if (!turretMotionPool.Has(firstTurretEntity) && !turretMotionPool.Has(secondTurretEntity))
                                {
                                    MoveTurret(firstTurretEntity, firstTurret.lineId, firstTurret.rowId, firstTurret.lineId - 1, firstTurret.rowId);

                                    if (secondTurretEntity != 0)
                                        MoveTurret(secondTurretEntity, firstTurret.lineId - 1, firstTurret.rowId, firstTurret.lineId, firstTurret.rowId);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (delta.y > 0)
                            Debug.Log("Swipe Up");
                        else
                            Debug.Log("Swipe Down");
                    }
                }

                if (touch.isEnded)
                    touch.isProcessed = true;
            }
        }

        void MoveTurret(int turretEntity, int fromLineId, int fromRowId, int toLineId, int toRowId)
        {
            Debug.Log($"MoveTurret {turretEntity} from {fromLineId}:{fromRowId} to {toLineId}:{toRowId}");

            ref var turretMotion = ref turretMotionPool.Add(turretEntity);
            turretMotion.timeTotal = MagicNumbersGame.turretMoveTime;
            turretMotion.fromLineId = fromLineId;
            turretMotion.fromRowId = fromRowId;
            turretMotion.toLineId = toLineId;
            turretMotion.toRowId = toRowId;

            ref var animationMarker = ref animationMarkerPool.GetOrAdd(turretEntity);
            animationMarker.usageCounter++;
        }

        int GetTurretEntityUnderTouch(Vector3 touchPos)
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(touchPos);

            if (Physics.Raycast(rayOrigin, out var hitInfo))
            {
                Debug.Log("Raycast hit object " + hitInfo.transform.name + " at the position of " + hitInfo.transform.position);

                var entityId = 0;
                var entityView = hitInfo.transform.GetComponentInParent<EntityView>();
                if (entityView != null)
                    entityId = entityView.entityId;

                if (turretPool.Has(entityId))
                {
                    return entityId;
                }
            }

            return 0;
        }

        int GetTurret(int lineId, int rowId)
        {
            if (lineId < 0 || lineId >= MagicNumbersGame.lineCount)
                return 0;

            for(var i = 0; i < context.turretsByLine[lineId].Count; i++)
            {
                var turretEntity = context.turretsByLine[lineId][i];
                ref var turret = ref turretPool.Get(turretEntity);
                if (turret.rowId == rowId)
                    return turretEntity;
            }

            return 0;
        }
    }
}
