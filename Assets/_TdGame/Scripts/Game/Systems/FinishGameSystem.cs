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
                }

                eventsWorld.DelEntity(entity);
            }
        }
    }
}
