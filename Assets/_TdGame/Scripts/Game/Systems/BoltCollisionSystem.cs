using Leopotam.EcsLite;

namespace TdGame
{
    sealed class BoltCollisionSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Bolt> boltPool;
        EcsPool<BoltTrigger> boltTriggerPool;
        EcsPool<Position> positionPool;
        EcsPool<Damage> damagePool;
        EcsPool<DestroyMarker> destroyMarkerPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            boltPool = world.GetPool<Bolt>();
            boltTriggerPool = world.GetPool<BoltTrigger>();
            positionPool = world.GetPool<Position>();
            damagePool = world.GetPool<Damage>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Bolt>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var bolt = ref boltPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                var creaturesByLine = context.creaturesByLine[position.lineId];
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
