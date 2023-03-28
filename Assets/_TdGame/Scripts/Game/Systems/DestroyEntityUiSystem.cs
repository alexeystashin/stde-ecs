using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class DestroyEntityUiSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<TurretUi> turretUiPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            turretUiPool = world.GetPool<TurretUi>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<DestroyMarker>().Inc<TurretUi>().End();

            foreach (int entity in filter)
            {
                ref var turretUi = ref turretUiPool.Get(entity);
                GameObject.Destroy(turretUi.hud.gameObject);
                turretUi.hud = null;
                turretUiPool.Del(entity);
            }
        }
    }
}
