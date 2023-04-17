using Pump.Unity;
using Zenject;

namespace TdGame
{
    public class CommonObjectsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<PrefabCache>()
                .AsSingle();
        }
    }
}