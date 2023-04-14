using Leopotam.EcsLite;
using System;
using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameWorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
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