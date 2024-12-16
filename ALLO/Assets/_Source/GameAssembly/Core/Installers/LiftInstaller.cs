using Company;
using Zenject;

namespace Core.Installers
{
    public class LiftInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLevelGeneration();
        }

        private void BindLevelGeneration()
        {
            Container.Bind<LiftLoading>()
                .AsSingle()
                .NonLazy();
        }
    }
}