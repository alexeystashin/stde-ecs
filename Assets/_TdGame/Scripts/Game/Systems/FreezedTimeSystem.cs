using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class FreezedTimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Freezed> freezedPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            freezedPool = world.GetPool<Freezed>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Freezed>().End();

            foreach (int entity in filter)
            {
                ref var freezed = ref freezedPool.Get(entity);

                if (freezed.time > 0)
                    freezed.time = Mathf.Max(0, freezed.time - Time.deltaTime);

                if (freezed.time <= 0)
                    freezedPool.Del(entity);
            }
        }
    }
}
