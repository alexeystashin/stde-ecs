using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class CreatureSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<CreatureSpawner> creatureSpawnerPool;
        EcsPool<Position> positionPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            creatureSpawnerPool = world.GetPool<CreatureSpawner>();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<CreatureSpawner>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var creatureSpawner = ref creatureSpawnerPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                if (creatureSpawner.cooldown > 0)
                    creatureSpawner.cooldown = Mathf.Max(0, creatureSpawner.cooldown - Time.deltaTime);

                if (creatureSpawner.cooldown <= 0)
                {
                    context.objectBuilder.SpawnCreature(position.lineId, position.z);
                    creatureSpawner.cooldown = Random.Range(MagicNumbersGame.creatureSpawnerCooldownMin, MagicNumbersGame.creatureSpawnerCooldownMax);
                }
            }
        }
    }
}
