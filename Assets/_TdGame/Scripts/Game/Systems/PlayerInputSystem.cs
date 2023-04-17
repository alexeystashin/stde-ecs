using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Turret> turretPool;
        EcsPool<TurretMotion> turretMotionPool;
        EcsPool<TurretFireTrigger> turretFireTriggerPool;
        EcsPool<AnimationMarker> animationMarkerPool;

        GameState gameState;
        GameRules gameRules;
        Camera gameCamera;
        GameInput gameInput;

        [Inject]
        void Construct(GameState gameState, GameRules gameRules, GameInput gameInput, Camera gameCamera)
        {
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.gameInput = gameInput;
            this.gameCamera = gameCamera;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            turretPool = world.GetPool<Turret>();
            turretMotionPool = world.GetPool<TurretMotion>();
            turretFireTriggerPool = world.GetPool<TurretFireTrigger>();
            animationMarkerPool = world.GetPool<AnimationMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            foreach (var touch in gameInput.touches)
            {
                if (touch.isProcessed)
                    continue;

                if (touch.isEnded)
                {
                    touch.isProcessed = true;

                    var touchedTurretEntityPacked = GetTurretEntityUnderTouch(touch.startTouchPos);
                    var currentTouchedTurretEntityPacked = GetTurretEntityUnderTouch(touch.currentTouchPos);

                    if (!touchedTurretEntityPacked.Unpack(world, out var touchedTurretEntity))
                        continue;

                    if (!currentTouchedTurretEntityPacked.Unpack(world, out var currentTouchedTurretEntity)
                        || touchedTurretEntity != currentTouchedTurretEntity)
                        continue;

                    turretFireTriggerPool.Add(touchedTurretEntity);

                    //Debug.Log($"Tap on {touchedTurretEntity}");
                }

                if (Vector3.Distance(touch.currentTouchPos, touch.startTouchPos) >= MagicNumbersGame.dragThresold * Screen.height)
                {
                    touch.isProcessed = true;

                    var touchedTurretEntityPacked = GetTurretEntityUnderTouch(touch.startTouchPos);

                    if (!touchedTurretEntityPacked.Unpack(world, out var touchedTurretEntity))
                        continue;

                    var delta = touch.currentTouchPos - touch.startTouchPos;
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        if (delta.x > 0)
                        {
                            //Debug.Log("Swipe Right");

                            ref var touchedTurret = ref turretPool.Get(touchedTurretEntity);
                            if (touchedTurret.lineId < MagicNumbersGame.lineCount - 1)
                            {
                                var secondTurretEntityPacked = GetTurret(touchedTurret.lineId + 1, touchedTurret.rowId);

                                if (!turretMotionPool.Has(touchedTurretEntity)
                                    && (!secondTurretEntityPacked.Unpack(world, out var secondTurretEntity) || !turretMotionPool.Has(secondTurretEntity)))
                                {
                                    MoveTurret(touchedTurretEntity, touchedTurret.lineId, touchedTurret.rowId, touchedTurret.lineId + 1, touchedTurret.rowId);

                                    if (secondTurretEntityPacked.Unpack(world, out secondTurretEntity))
                                        MoveTurret(secondTurretEntity, touchedTurret.lineId + 1, touchedTurret.rowId, touchedTurret.lineId, touchedTurret.rowId);
                                }
                            }
                        }
                        else
                        {
                            //Debug.Log("Swipe Left");

                            ref var touchedTurret = ref turretPool.Get(touchedTurretEntity);
                            if (touchedTurret.lineId > 0)
                            {
                                var secondTurretEntityPacked = GetTurret(touchedTurret.lineId - 1, touchedTurret.rowId);

                                if (!turretMotionPool.Has(touchedTurretEntity)
                                    && (!secondTurretEntityPacked.Unpack(world, out var secondTurretEntity) || !turretMotionPool.Has(secondTurretEntity)))
                                {
                                    MoveTurret(touchedTurretEntity, touchedTurret.lineId, touchedTurret.rowId, touchedTurret.lineId - 1, touchedTurret.rowId);

                                    if (secondTurretEntityPacked.Unpack(world, out secondTurretEntity))
                                        MoveTurret(secondTurretEntity, touchedTurret.lineId - 1, touchedTurret.rowId, touchedTurret.lineId, touchedTurret.rowId);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (delta.y > 0)
                        {
                            //Debug.Log("Swipe Up");

                            ref var touchedTurret = ref turretPool.Get(touchedTurretEntity);
                            if (touchedTurret.rowId < gameRules.towerLines - 1)
                            {
                                var secondTurretEntityPacked = GetTurret(touchedTurret.lineId, touchedTurret.rowId + 1);

                                if (!turretMotionPool.Has(touchedTurretEntity)
                                    && (!secondTurretEntityPacked.Unpack(world, out var secondTurretEntity) || !turretMotionPool.Has(secondTurretEntity)))
                                {
                                    MoveTurret(touchedTurretEntity, touchedTurret.lineId, touchedTurret.rowId, touchedTurret.lineId, touchedTurret.rowId + 1);

                                    if (secondTurretEntityPacked.Unpack(world, out secondTurretEntity))
                                        MoveTurret(secondTurretEntity, touchedTurret.lineId, touchedTurret.rowId + 1, touchedTurret.lineId, touchedTurret.rowId);
                                }
                            }
                        }
                        else
                        {
                            //Debug.Log("Swipe Down");

                            ref var touchedTurret = ref turretPool.Get(touchedTurretEntity);
                            if (touchedTurret.rowId > 0)
                            {
                                var secondTurretEntityPacked = GetTurret(touchedTurret.lineId, touchedTurret.rowId - 1);

                                if (!turretMotionPool.Has(touchedTurretEntity)
                                    && (!secondTurretEntityPacked.Unpack(world, out var secondTurretEntity) || !turretMotionPool.Has(secondTurretEntity)))
                                {
                                    MoveTurret(touchedTurretEntity, touchedTurret.lineId, touchedTurret.rowId, touchedTurret.lineId, touchedTurret.rowId - 1);

                                    if (secondTurretEntityPacked.Unpack(world, out secondTurretEntity))
                                        MoveTurret(secondTurretEntity, touchedTurret.lineId, touchedTurret.rowId - 1, touchedTurret.lineId, touchedTurret.rowId);
                                }
                            }
                        }
                    }
                }

                if (touch.isEnded)
                    touch.isProcessed = true;
            }
        }

        void MoveTurret(int turretEntity, int fromLineId, int fromRowId, int toLineId, int toRowId)
        {
            //Debug.Log($"MoveTurret {turretEntity} from {fromLineId}:{fromRowId} to {toLineId}:{toRowId}");

            ref var turretMotion = ref turretMotionPool.Add(turretEntity);
            turretMotion.timeTotal = MagicNumbersGame.turretMoveTime;
            turretMotion.fromLineId = fromLineId;
            turretMotion.fromRowId = fromRowId;
            turretMotion.toLineId = toLineId;
            turretMotion.toRowId = toRowId;

            ref var animationMarker = ref animationMarkerPool.GetOrAdd(turretEntity);
            animationMarker.usageCounter++;
        }

        EcsPackedEntity GetTurretEntityUnderTouch(Vector3 touchPos)
        {
            Ray rayOrigin = gameCamera.ScreenPointToRay(touchPos);

            if (Physics.Raycast(rayOrigin, out var hitInfo))
            {
                //Debug.Log("Raycast hit object " + hitInfo.transform.name + " at the position of " + hitInfo.transform.position);

                EcsPackedEntity entityIdPacked = default;
                var entityView = hitInfo.transform.GetComponentInParent<EntityView>();
                if (entityView != null)
                    entityIdPacked = entityView.entityId;

                if (entityIdPacked.Unpack(world, out var entityId) && turretPool.Has(entityId))
                {
                    return entityIdPacked;
                }
            }

            return default;
        }

        EcsPackedEntity GetTurret(int lineId, int rowId)
        {
            if (lineId < 0 || lineId >= MagicNumbersGame.lineCount)
                return default;

            for(var i = 0; i < gameState.turretsByLine[lineId].Count; i++)
            {
                var turretEntity = gameState.turretsByLine[lineId][i];
                ref var turret = ref turretPool.Get(turretEntity);
                if (turret.rowId == rowId)
                    return world.PackEntity(turretEntity);
            }

            return default;
        }
    }
}
