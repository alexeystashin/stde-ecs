using Leopotam.EcsLite;
using Pump.Unity;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class CooldownSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Cooldown> cooldownPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Cooldown>().End();

            foreach (int entity in filter)
            {
                ref var cooldown = ref cooldownPool.Get(entity);

                if (cooldown.cooldown > 0)
                    cooldown.cooldown = Mathf.Max(0, cooldown.cooldown - PumpTime.deltaTime);
            }
        }
    }
}
