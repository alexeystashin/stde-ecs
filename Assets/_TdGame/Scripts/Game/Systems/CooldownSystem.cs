using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class CooldownSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Cooldown> cooldownPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Cooldown>().End();

            foreach (int entity in filter)
            {
                ref var cooldown = ref cooldownPool.Get(entity);

                if (cooldown.cooldown > 0)
                    cooldown.cooldown = Mathf.Max(0, cooldown.cooldown - Time.deltaTime);
            }
        }
    }
}
