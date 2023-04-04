using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class AddScoreForKillSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Creature> creaturePool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            creaturePool = world.GetPool<Creature>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<DestroyMarker>().Inc<Creature>().End();

            foreach (int entity in filter)
            {
                ref var creature = ref creaturePool.Get(entity);

                gameState.score += creature.killScore;
            }
        }
    }
}
