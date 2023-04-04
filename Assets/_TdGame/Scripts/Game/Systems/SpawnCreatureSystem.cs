using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class SpawnCreatureSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Spawner> spawnerPool;
        EcsPool<Position> positionPool;
        EcsPool<Cooldown> cooldownPool;

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
            spawnerPool = world.GetPool<Spawner>();
            positionPool = world.GetPool<Position>();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Spawner>().Exc<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                ref var spawner = ref spawnerPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);

                if (cooldown.cooldown <= 0)
                {
                    var creatureTemplate = staticGameData.creatures[spawner.template.creatureId];
                    objectBuilder.CreateCreature(creatureTemplate, position.lineId, position.z);
                    cooldown.cooldown = Random.Range(spawner.template.cooldownMin, spawner.template.cooldownMin);
                }
            }
        }
    }
}
