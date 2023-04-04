using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameInputInstaller : MonoInstaller
    {
        [SerializeField] GameInput gameInput;

        public override void InstallBindings()
        {
            Container.Bind<GameInput>().FromInstance(gameInput).AsSingle();
        }
    }
}