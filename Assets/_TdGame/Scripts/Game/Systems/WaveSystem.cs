using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class WaveSystem : IEcsInitSystem, IEcsRunSystem
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
            if (context.currentWave >= context.gameRules.waves.Count)
                return;

            var filter = world.Filter<Spawner>().Exc<DestroyMarker>().End();

            if (filter.GetEntitiesCount() == 0)
            {
                context.currentWave++;
                if (context.currentWave < context.gameRules.waves.Count)
                {
                    Debug.Log($"Wave {context.currentWave + 1}/{context.gameRules.waves.Count} started");
                    context.objectBuilder.CreateWaveSpawners(context.gameRules.waves[context.currentWave]);
                }
            }
        }
    }
}
