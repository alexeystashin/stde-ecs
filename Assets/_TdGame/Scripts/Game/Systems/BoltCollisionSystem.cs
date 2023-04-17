using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class BoltCollisionSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Bolt> boltPool;
        EcsPool<BoltTrigger> boltTriggerPool;
        EcsPool<Position> positionPool;
        EcsPool<Damage> damagePool;
        EcsPool<DestroyMarker> destroyMarkerPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            boltPool = world.GetPool<Bolt>();
            boltTriggerPool = world.GetPool<BoltTrigger>();
            positionPool = world.GetPool<Position>();
            damagePool = world.GetPool<Damage>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Bolt>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var bolt = ref boltPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                var creaturesByLine = gameState.creaturesByLine[position.lineId];
                for (var i = 0; i < creaturesByLine.Count; i++)
                {
                    var creatureEntity = creaturesByLine[i];
                    if (destroyMarkerPool.Has(creatureEntity))
                        continue;

                    ref var creaturePosition = ref positionPool.Get(creatureEntity);
                    if (position.z >= creaturePosition.z)
                    {
                        ref var boltTrigger = ref boltTriggerPool.Add(entity);
                        boltTrigger.targetEntity = world.PackEntity(creatureEntity);

                        destroyMarkerPool.Add(entity);
                        break;
                    }
                }
            }
        }
    }
}
