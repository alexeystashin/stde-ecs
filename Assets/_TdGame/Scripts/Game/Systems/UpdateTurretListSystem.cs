using Leopotam.EcsLite;

namespace TdGame
{
    sealed class UpdateTurretListSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Position> positionPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Turret>().Inc<Position>().Exc<DestroyMarker>().End();

            for (var i = 0; i < context.turretsByLine.Count; i++)
            {
                context.turretsByLine[i].Clear();
            }

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                context.turretsByLine[position.lineId].Add(entity);
            }
        }
    }
}
