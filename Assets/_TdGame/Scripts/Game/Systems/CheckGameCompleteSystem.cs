using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class CheckGameCompleteSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld eventsWorld;
        EcsPool<GameFinishedEvent> gameFinishedEventPool;

        GameState gameState;
        GameRules gameRules;

        [Inject]
        void Construct(GameState gameState, GameRules gameRules)
        {
            this.gameState = gameState;
            this.gameRules = gameRules;
        }

        public void Init(IEcsSystems systems)
        {
            eventsWorld = systems.GetWorld("events");
            gameFinishedEventPool = eventsWorld.GetPool<GameFinishedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (gameState.currentWave >= gameRules.waves.Count && gameState.creaturesByLine.All(line => line.Count == 0))
            {
                var eventEntity = eventsWorld.NewEntity();

                ref var gameFinishedEvent = ref gameFinishedEventPool.Add(eventEntity);
                gameFinishedEvent.isWin = true;
            }
        }
    }
}
