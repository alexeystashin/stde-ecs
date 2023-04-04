using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class ApplyAreaBoltSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<AreaBolt> areaBoltPool;
        EcsPool<BoltTrigger> boltTriggerPool;
        EcsPool<Position> positionPool;

        StaticGameData staticGameData;
        GameObjectBuilder objectBuilder;

        [Inject]
        void Construct(StaticGameData staticGameData, GameObjectBuilder objectBuilder)
        {
            this.staticGameData = staticGameData;
            this.objectBuilder = objectBuilder;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            areaBoltPool = world.GetPool<AreaBolt>();
            boltTriggerPool = world.GetPool<BoltTrigger>();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<BoltTrigger>().Inc<AreaBolt>().End();

            foreach (int entity in filter)
            {
                ref var areaBolt = ref areaBoltPool.GetOrAdd(entity);
                ref var boltTrigger = ref boltTriggerPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                if (!boltTrigger.targetEntity.Unpack(world, out var targetEntity))
                    continue;

                //Debug.Log($"Area spawned {areaBolt.areaTemplateId}");
                var areaTemplate = staticGameData.areas[areaBolt.areaTemplateId];
                objectBuilder.CreateArea(areaTemplate, position.lineId, position.z);
            }
        }
    }
}
