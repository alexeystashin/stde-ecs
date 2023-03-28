using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class UpdateTurretHudSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<TurretUi> turretUiPool;
        EcsPool<View> viewPool;
        EcsPool<Turret> turretPool;
        EcsPool<Cooldown> cooldownPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
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

                var hudPosition = RectTransformUtility.WorldToScreenPoint(context.camera, view.viewObject.transform.position);
                turretUi.hud.transform.position = hudPosition;

                var loadProgress = 1f - (cooldown.cooldown / turret.template.attackCooldown);
                turretUi.hud.loadProgressBar.fillAmount = loadProgress;
                turretUi.hud.readySign.SetActive(!turret.template.autoAttack && loadProgress >= 1f);
            }
        }
    }
}
