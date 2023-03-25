using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class EntityMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Motion> motionPool;
        EcsPool<Position> positionPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            motionPool = world.GetPool<Motion>();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Motion>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var motion = ref motionPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                position.z += motion.velocityZ * Time.deltaTime;
            }
        }
    }
}
