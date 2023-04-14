using Common;
using UnityEngine;
using Zenject;

namespace TdGame
{
    public class UnityObjectsInstaller : MonoInstaller
    {
        [SerializeField] Camera gameCamera;
        [SerializeField] Canvas gameCanvas;

        [SerializeField] PrefabCache prefabCache;

        [SerializeField] GameInput gameInput;
        [SerializeField] GameUi gameUi;

        [SerializeField] Transform rootTransform;

        public override void InstallBindings()
        {
            Container.Bind<Camera>()
                .FromInstance(gameCamera)
                .AsSingle();

            Container
                .Bind<Canvas>()
                .FromInstance(gameCanvas)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PrefabCache>()
                .AsSingle();

            Container
                .Bind<GameInput>()
                .FromInstance(gameInput)
                .AsSingle();

            Container
                .Bind<GameUi>()
                .FromInstance(gameUi)
                .AsSingle();

            Container
                .Bind<MemoryPoolSettings>()
                .FromInstance(MemoryPoolSettings.Default)
                .AsSingle();

            Container
                .Bind<Transform>()
                .FromInstance(rootTransform)
                .AsSingle()
                .WhenInjectedInto<EntityViewFactory>();

            Container
                .BindFactory<string, EntityView, EntityView.Factory>()
                .FromFactory<EntityViewFactory>();

            Container
                .Bind<EntityView.Pool>()
                .AsSingle();
        }
    }
}