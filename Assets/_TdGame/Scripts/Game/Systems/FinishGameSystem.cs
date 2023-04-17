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

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            eventsWorld = systems.GetWorld("events");
            gameFinishedEventPool = eventsWorld.GetPool<GameFinishedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (gameState.isGameFinished)
                return;

            var filter = eventsWorld.Filter<GameFinishedEvent>().End();

            foreach (int entity in filter)
            {
                ref var gameFinishedEvent = ref gameFinishedEventPool.Get(entity);

                if (!gameState.isGameFinished)
                {
                    Debug.Log($"GameFinished {(gameFinishedEvent.isGameWon ? "WIN" : "LOSE")}");
                    if (gameFinishedEvent.isGameWon)
                        gameState.isGameWon = true;
                    else
                        gameState.isGameLost = true;

                    gameState.gamePauseCounter++;
                }
            }
        }
    }
}
