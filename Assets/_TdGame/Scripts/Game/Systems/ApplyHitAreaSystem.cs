using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class ApplyHitAreaSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<HitArea> hitAreaPool;
        EcsPool<AreaTrigger> areaTriggerPool;
        EcsPool<Cooldown> cooldownPool;
        EcsPool<Position> positionPool;
        EcsPool<Damage> damagePool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            hitAreaPool = world.GetPool<HitArea>();
            areaTriggerPool = world.GetPool<AreaTrigger>();
            cooldownPool = world.GetPool<Cooldown>();
            positionPool = world.GetPool<Position>();
            damagePool = world.GetPool<Damage>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<AreaTrigger>().Inc<HitArea>().End();
            var creaturesFilter = world.Filter<Creature>().Exc<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                ref var hitArea = ref hitAreaPool.GetOrAdd(entity);
                ref var areaTrigger = ref areaTriggerPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                cooldown.cooldown = hitArea.cooldown;

                foreach (int creatureEntity in creaturesFilter)
                {
                    ref var creaturePosition = ref positionPool.Get(creatureEntity);

                    if (creaturePosition.lineId == position.lineId && Mathf.Abs(creaturePosition.z - position.z) <= hitArea.size * 0.5f)
                    {
                        //Debug.Log($"Hit creature {creatureEntity} for {hitArea.hitPower} points");
                        ref var targetDamage = ref damagePool.GetOrAdd(creatureEntity);
                        targetDamage.damage += hitArea.hitPower;
                    }
                }
            }
        }
    }
}
