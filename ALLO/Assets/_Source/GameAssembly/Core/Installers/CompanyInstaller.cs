using Company;
using Zenject;

namespace Core.Installers
{
    public class CompanyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //BindCompany();
        }

        private void BindCompany()
        {
            Container.Bind<CompanyInterLevelDataContainer>()
                .AsSingle()
                .NonLazy();
        }
    }
}