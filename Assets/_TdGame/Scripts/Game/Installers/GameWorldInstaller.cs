using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameWorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EcsWorld>().FromNew().AsSingle();
            Container.Bind<GameState>().FromNew().AsSingle();
            Container.Bind<StaticGameData>().FromMethod(MockStaticGameData.Create).AsSingle();
            Container.Bind<GameRules>().FromMethod(CreateGameRules).AsSingle();
            Container.Bind<GameObjectBuilder>().FromNew().AsSingle();
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
    }
}