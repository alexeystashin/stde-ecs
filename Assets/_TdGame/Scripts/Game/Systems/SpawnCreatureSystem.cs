using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class SpawnCreatureSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Spawner> spawnerPool;
        EcsPool<Position> positionPool;
        EcsPool<Cooldown> cooldownPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
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
                    var creatureTemplate = context.staticGameData.creatures[spawner.template.creatureId];
                    context.objectBuilder.CreateCreature(creatureTemplate, position.lineId, position.z);
                    cooldown.cooldown = Random.Range(spawner.template.cooldownMin, spawner.template.cooldownMin);
                }
            }
        }
    }
}
