using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;

namespace TdGame
{
    sealed class CheckGameCompleteSystem : IEcsInitSystem, IEcsRunSystem
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
            if (context.currentWave >= context.gameRules.waves.Count && context.creaturesByLine.All(line => line.Count == 0))
            {
                var eventEntity = eventsWorld.NewEntity();

                ref var gameFinishedEvent = ref gameFinishedEventPool.Add(eventEntity);
                gameFinishedEvent.isWin = true;
            }
        }
    }
}
