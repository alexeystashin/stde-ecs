using Leopotam.EcsLite;

namespace TdGame
{
    sealed class AddScoreForKillSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Creature> creaturePool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            creaturePool = world.GetPool<Creature>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<DestroyMarker>().Inc<Creature>().End();

            foreach (int entity in filter)
            {
                ref var creature = ref creaturePool.Get(entity);

                context.score += creature.killScore;
            }
        }
    }
}
