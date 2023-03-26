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

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            spawnerPool = world.GetPool<Spawner>();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Spawner>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var spawner = ref spawnerPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                if (spawner.cooldown > 0)
                    spawner.cooldown = Mathf.Max(0, spawner.cooldown - Time.deltaTime);

                if (spawner.cooldown <= 0)
                {
                    context.objectBuilder.SpawnCreature(position.lineId, position.z);
                    spawner.cooldown = Random.Range(MagicNumbersGame.spawnerCooldownMin, MagicNumbersGame.spawnerCooldownMax);
                }
            }
        }
    }
}
