using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class CreatureArriveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsWorld eventsWorld;
        EcsPool<Position> positionPool;
        EcsPool<DestroyMarker> destroyMarkerPool;
        EcsPool<GameFinishedEvent> gameFinishedEventPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventsWorld = systems.GetWorld("events");
            positionPool = world.GetPool<Position>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
            gameFinishedEventPool = eventsWorld.GetPool<GameFinishedEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Creature>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                if (position.z <= MagicNumbersGame.creatureArriveZ)
                {
                    destroyMarkerPool.Add(entity);

                    var eventEntity = eventsWorld.NewEntity();
                    ref var gameFinishedEvent = ref gameFinishedEventPool.Add(eventEntity);
                    gameFinishedEvent.isWin = false;
                }
            }
        }
    }
}
