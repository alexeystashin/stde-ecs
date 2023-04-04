using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class UpdateTurretHudSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<TurretUi> turretUiPool;
        EcsPool<View> viewPool;
        EcsPool<Turret> turretPool;
        EcsPool<Cooldown> cooldownPool;

        Camera gameCamera;

        [Inject]
        void Construct(Camera gameCamera)
        {
            this.gameCamera = gameCamera;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            turretUiPool = world.GetPool<TurretUi>();
            viewPool = world.GetPool<View>();
            turretPool = world.GetPool<Turret>();
            cooldownPool = world.GetPool<Cooldown>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<TurretUi>().Inc<View>().End();

            foreach (int entity in filter)
            {
                ref var turretUi = ref turretUiPool.Get(entity);
                ref var view = ref viewPool.Get(entity);
                ref var turret = ref turretPool.Get(entity);
                ref var cooldown = ref cooldownPool.Get(entity);

                if (view.viewObject == null || turretUi.hud == null)
                    continue;

                var hudPosition = RectTransformUtility.WorldToScreenPoint(gameCamera, view.viewObject.transform.position);
                turretUi.hud.transform.position = hudPosition;

                var loadProgress = 1f - (cooldown.cooldown / turret.template.attackCooldown);
                turretUi.hud.loadProgressBar.fillAmount = loadProgress;
                turretUi.hud.readySign.SetActive(!turret.template.autoAttack && loadProgress >= 1f);
            }
        }
    }
}
