using Common;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class FinishGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld eventsWorld;
        EcsPool<GameFinishedEvent> gameFinishedEventPool;

        GameState gameState;
        GameRules gameRules;
        Canvas gameCanvas;
        PrefabCache prefabCache;

        [Inject]
        void Construct(GameState gameState, GameRules gameRules, Canvas gameCanvas, PrefabCache prefabCache)
        {
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.gameCanvas = gameCanvas;
            this.prefabCache = prefabCache;
        }

        public void Init(IEcsSystems systems)
        {
            eventsWorld = systems.GetWorld("events");
            gameFinishedEventPool = eventsWorld.GetPool<GameFinishedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = eventsWorld.Filter<GameFinishedEvent>().End();

            foreach (int entity in filter)
            {
                ref var gameFinishedEvent = ref gameFinishedEventPool.Get(entity);

                if (!gameState.isGameFinished)
                {
                    Debug.Log($"GameFinished {(gameFinishedEvent.isWin ? "WIN" : "LOSE")}");
                    gameState.isGameFinished = true;
                    gameState.isWin = gameFinishedEvent.isWin;

                    // todo: move window initialization out of here
                    var go = GameObject.Instantiate(prefabCache.GetPrefab(GamePrefabPath.gameResultWindow), gameCanvas.GetComponent<RectTransform>());
                    var gameResultWindow = go.GetComponent<GameResultWindow>();
                    gameResultWindow.titleText.text = gameState.isWin ? "WIN!" : "LOSE!";
                    gameResultWindow.scoreText.text = $"Score: {gameState.score}";
                    var totalWaves = gameRules.waves.Count;
                    var currentWave = Mathf.Min(gameState.currentWave + 1, totalWaves);
                    gameResultWindow.waveCounterText.text = $"Wave {currentWave}/{totalWaves}";
                }
            }
        }
    }
}
