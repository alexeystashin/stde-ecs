using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class ApplyHitBoltSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<HitBolt> hitBoltPool;
        EcsPool<BoltTrigger> boltTriggerPool;
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
            hitBoltPool = world.GetPool<HitBolt>();
            boltTriggerPool = world.GetPool<BoltTrigger>();
            damagePool = world.GetPool<Damage>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<BoltTrigger>().Inc<HitBolt>().End();

            foreach (int entity in filter)
            {
                ref var hitBolt = ref hitBoltPool.GetOrAdd(entity);
                ref var boltTrigger = ref boltTriggerPool.Get(entity);

                if (!boltTrigger.targetEntity.Unpack(world, out var targetEntity))
                    continue;

                ref var targetDamage = ref damagePool.GetOrAdd(targetEntity);
                targetDamage.damage += hitBolt.hitPower;
            }
        }
    }
}
