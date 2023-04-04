using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameUiInstaller : MonoInstaller
    {
        [SerializeField] GameUi gameUi;

        public override void InstallBindings()
        {
            Container.Bind<GameUi>().FromInstance(gameUi).AsSingle();
        }
    }
}