using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class UpdateTurretListSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Position> positionPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Turret>().Inc<Position>().Exc<DestroyMarker>().End();

            for (var i = 0; i < gameState.turretsByLine.Count; i++)
            {
                gameState.turretsByLine[i].Clear();
            }

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                gameState.turretsByLine[position.lineId].Add(entity);
            }
        }
    }
}
