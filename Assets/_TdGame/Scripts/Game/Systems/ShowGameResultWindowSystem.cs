using Leopotam.EcsLite;
using Pump.Unity;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class ShowGameResultWindowSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld eventsWorld;
        EcsPool<GameFinishedEvent> gameFinishedEventPool;

        DiContainer di;
        GameState gameState;
        GameRules gameRules;
        Canvas gameCanvas;
        IUnityPrefabProvider prefabProvider;

        bool isWindowShowed;

        [Inject]
        void Construct(DiContainer di, GameState gameState, GameRules gameRules, Canvas gameCanvas, IUnityPrefabProvider prefabProvider)
        {
            this.di = di;
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.gameCanvas = gameCanvas;
            this.prefabProvider = prefabProvider;
        }

        public void Init(IEcsSystems systems)
        {
            eventsWorld = systems.GetWorld("events");
            gameFinishedEventPool = eventsWorld.GetPool<GameFinishedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (isWindowShowed || gameState.isGameFinished)
                return;

            var filter = eventsWorld.Filter<GameFinishedEvent>().End();

            foreach (int entity in filter)
            {
                ref var gameFinishedEvent = ref gameFinishedEventPool.Get(entity);

                var go = di.InstantiatePrefab(prefabProvider.GetPrefab(GamePrefabPath.gameResultWindow), gameCanvas.GetComponent<RectTransform>());
                var gameResultWindow = go.GetComponent<GameResultWindow>();
                gameResultWindow.titleText.text = gameFinishedEvent.isGameWon ? "WIN!" : "LOSE!";
                gameResultWindow.scoreText.text = $"Score: {gameState.score}";
                var totalWaves = gameRules.waves.Count;
                var currentWave = Mathf.Min(gameState.currentWave + 1, totalWaves);
                gameResultWindow.waveCounterText.text = $"Wave {currentWave}/{totalWaves}";

                isWindowShowed = true;
                break;
            }
        }
    }
}
