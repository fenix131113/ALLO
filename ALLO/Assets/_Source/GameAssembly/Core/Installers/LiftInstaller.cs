using Company;
using PlayerSystem;
using Zenject;

namespace Core.Installers
{
    public class LiftInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLift();
        }
        private void BindLift()
        {
            Container.BindInterfacesAndSelfTo<LiftLoading>()
                .AsSingle()
                .NonLazy();
        }
    }
}