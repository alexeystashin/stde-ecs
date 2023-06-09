using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class ApplyDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Health> healthPool;
        EcsPool<Damage> damagePool;
        EcsPool<DestroyMarker> destroyMarkerPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            healthPool = world.GetPool<Health>();
            damagePool = world.GetPool<Damage>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameState.isGameRunning)
                return;

            var filter = world.Filter<Damage>().Inc<Health>().Exc<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                ref var health = ref healthPool.Get(entity);
                ref var damage = ref damagePool.Get(entity);

                health.health = Mathf.Max(0, health.health - damage.damage);

                damagePool.Del(entity);

                if (health.health <= 0)
                    destroyMarkerPool.Add(entity);
            }
        }
    }
}
