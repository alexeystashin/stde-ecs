using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameCameraInstaller : MonoInstaller
    {
        [SerializeField] Camera gameCamera;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(gameCamera).AsSingle();
        }
    }
}
