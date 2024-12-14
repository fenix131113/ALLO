using Company;
using Zenject;

namespace Core.Installers
{
    public class PlayerCompanyLinkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSaver();
        }

        private void BindSaver()
        {
            Container.BindInterfacesAndSelfTo<CompanyDataSaveLoader>()
                .AsSingle()
                .NonLazy();
        }
    }
}