using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class ApplyFreezeAreaSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<FreezeArea> freezeAreaPool;
        EcsPool<AreaTrigger> areaTriggerPool;
        EcsPool<Cooldown> cooldownPool;
        EcsPool<Position> positionPool;
        EcsPool<Freezed> freezedPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            freezeAreaPool = world.GetPool<FreezeArea>();
            areaTriggerPool = world.GetPool<AreaTrigger>();
            cooldownPool = world.GetPool<Cooldown>();
            positionPool = world.GetPool<Position>();
            freezedPool = world.GetPool<Freezed>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<AreaTrigger>().Inc<FreezeArea>().End();
            var creaturesFilter = world.Filter<Creature>().Exc<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                ref var freezeArea = ref freezeAreaPool.GetOrAdd(entity);
                ref var areaTrigger = ref areaTriggerPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                cooldown.cooldown = freezeArea.cooldown;

                foreach (int creatureEntity in creaturesFilter)
                {
                    ref var creaturePosition = ref positionPool.Get(creatureEntity);

                    if (creaturePosition.lineId == position.lineId && Mathf.Abs(creaturePosition.z - position.z) <= freezeArea.size * 0.5f)
                    {
                        var freezeTime = 1.0f; // todo: make configurable
                        //Debug.Log($"Freeze creature {creatureEntity} for {freezeTime} points");
                        ref var targetFreezed = ref freezedPool.GetOrAdd(creatureEntity);
                        targetFreezed.time = freezeTime;
                    }
                }
            }
        }
    }
}
