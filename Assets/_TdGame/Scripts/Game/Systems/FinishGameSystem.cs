using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class FinishGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld eventsWorld;
        EcsPool<GameFinishedEvent> gameFinishedEventPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            eventsWorld = systems.GetWorld("events");
            gameFinishedEventPool = eventsWorld.GetPool<GameFinishedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = eventsWorld.Filter<GameFinishedEvent>().End();

            foreach (int entity in filter)
            {
                ref var gameFinishedEvent = ref gameFinishedEventPool.Get(entity);

                if (!context.isGameFinished)
                {
                    Debug.Log($"GameFinished {(gameFinishedEvent.isWin ? "WIN" : "LOSE")}");
                    context.isGameFinished = true;
                    context.isWin = gameFinishedEvent.isWin;

                    // todo: move window initialization out of here
                    var go = GameObject.Instantiate(PrefabCache.instance.GetPrefab(GamePrefabPath.gameResultWindow), context.gameCanvas.GetComponent<RectTransform>());
                    var gameResultWindow = go.GetComponent<GameResultWindow>();
                    gameResultWindow.titleText.text = context.isWin ? "WIN!" : "LOSE!";
                    gameResultWindow.scoreText.text = $"Score: {context.score}";
                    var totalWaves = context.gameRules.waves.Count;
                    var currentWave = Mathf.Min(context.currentWave + 1, totalWaves);
                    gameResultWindow.waveCounterText.text = $"Wave {currentWave}/{totalWaves}";
                }
            }
        }
    }
}
