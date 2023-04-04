using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class WaveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;

        GameState gameState;
        GameRules gameRules;
        GameObjectBuilder objectBuilder;

        [Inject]
        void Construct(GameState gameState, GameRules gameRules, GameObjectBuilder objectBuilder)
        {
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.objectBuilder = objectBuilder;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            if (gameState.currentWave >= gameRules.waves.Count)
                return;

            gameState.currentWaveTime += Time.deltaTime;

            var filter = world.Filter<Spawner>().Exc<DestroyMarker>().End();

            if (filter.GetEntitiesCount() == 0)
            {
                gameState.currentWave++;
                if (gameState.currentWave >= gameRules.waves.Count)
                    return;

                Debug.Log($"Wave {gameState.currentWave + 1}/{gameRules.waves.Count} started");
                objectBuilder.CreateWaveSpawners(gameRules.waves[gameState.currentWave]);

                // calculate wave time (duplicate in GameStartup & WaveSystem!)
                gameState.currentWaveTime = 0;
                gameState.currentWaveTimeTotal = 0;
                foreach(var line in gameRules.waves[gameState.currentWave].lineSpawners)
                {
                    foreach(var spawnerTemplate in line)
                    {
                        gameState.currentWaveTimeTotal = Mathf.Max(gameState.currentWaveTimeTotal,
                            spawnerTemplate.delay + spawnerTemplate.lifetime);
                    }
                }
            }
        }
    }
}
