using Leopotam.EcsLite;
using Pump.Unity;
using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameObjectsInstaller : MonoInstaller
    {
        [SerializeField] Camera gameCamera;
        [SerializeField] Canvas gameCanvas;

        [SerializeField] GameUi gameUi;

        [SerializeField] Transform rootTransform;

        IUnityPrefabProvider prefabProvider;

        [Inject]
        void Construct(IUnityPrefabProvider prefabProvider)
        {
            this.prefabProvider = prefabProvider;
        }

        public override void InstallBindings()
        {
            Container
                .Bind<Camera>()
                .FromInstance(gameCamera)
                .AsSingle();

            Container
                .Bind<Canvas>()
                .FromInstance(gameCanvas)
                .AsSingle();

            Container
                .Bind<EcsWorld>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<GameState>()
                .AsSingle();

            Container
                .Bind<StaticGameData>()
                .FromMethod(MockStaticGameData.Create)
                .AsSingle();

            Container
                .Bind<GameRules>()
                .FromMethod(CreateGameRules)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<GameObjectBuilder>()
                .AsSingle();

            Container
                 .Bind<GameInput>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .Bind<GameUi>()
                .FromInstance(gameUi)
                .AsSingle();

            Container
                .Bind<GameTime>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            Container
                .Bind<MemoryPoolSettings>()
                .FromInstance(MemoryPoolSettings.Default)
                .AsSingle();

            Container
                .BindFactory<string, EntityView, EntityView.Factory>()
                .FromMethod(CreateEntityView);

            Container
                .Bind<EntityView.Pool>()
                .AsSingle();
        }

        GameRules CreateGameRules()
        {
            //todo: change rules setup
            var level = PlayerPrefs.GetInt("level");
            if (level <= 0)
                return MockStaticGameData.CreateTutorialGameRules();
            else if (level == 1)
                return MockStaticGameData.CreateEasyGameRules();
            else if (level == 2)
                return MockStaticGameData.CreateMediumGameRules();
            else
                return MockStaticGameData.CreateHardGameRules();
        }

        private EntityView CreateEntityView(DiContainer di, string prefabPath)
        {
            Debug.Log($"Create new EntityView {prefabPath}");

            var prefab = prefabProvider.GetPrefab(prefabPath);
            var go = di.InstantiatePrefab(prefab, rootTransform);
            var entityView = go.GetComponent<EntityView>();
            if (entityView == null)
            {
                entityView = go.AddComponent<EntityView>();
                di.Inject(entityView);
            }

            PumpTimeUtils.AddTimeWatcher(go.transform);

            return entityView;
        }
    }
}