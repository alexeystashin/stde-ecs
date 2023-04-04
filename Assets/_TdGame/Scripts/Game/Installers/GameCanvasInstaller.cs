using UnityEngine;
using Zenject;

namespace TdGame
{
    public class GameCanvasInstaller : MonoInstaller
    {
        [SerializeField] Canvas gameCanvas;

        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromInstance(gameCanvas).AsSingle();
        }
    }
}