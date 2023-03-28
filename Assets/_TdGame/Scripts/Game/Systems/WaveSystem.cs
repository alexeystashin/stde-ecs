using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class WaveSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            if (context.currentWave >= context.gameRules.waves.Count)
                return;

            context.currentWaveTime += Time.deltaTime;

            var filter = world.Filter<Spawner>().Exc<DestroyMarker>().End();

            if (filter.GetEntitiesCount() == 0)
            {
                context.currentWave++;
                if (context.currentWave >= context.gameRules.waves.Count)
                    return;

                Debug.Log($"Wave {context.currentWave + 1}/{context.gameRules.waves.Count} started");
                context.objectBuilder.CreateWaveSpawners(context.gameRules.waves[context.currentWave]);

                // calculate wave time (duplicate in GameStartup & WaveSystem!)
                context.currentWaveTime = 0;
                context.currentWaveTimeTotal = 0;
                foreach(var line in context.gameRules.waves[context.currentWave].lineSpawners)
                {
                    foreach(var spawnerTemplate in line)
                    {
                        context.currentWaveTimeTotal = Mathf.Max(context.currentWaveTimeTotal,
                            spawnerTemplate.delay + spawnerTemplate.lifetime);
                    }
                }
            }
        }
    }
}
