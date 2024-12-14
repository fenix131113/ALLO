using Company;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class CompanyInstaller : MonoInstaller
    {
        private CompanyInterLevelDataContainer _dataContainer;

        public override void InstallBindings()
        {
            BindCompany();
        }

        private void BindCompany()
        {
            _dataContainer = FindObjectOfType<CompanyInterLevelDataContainer>();
            
            if (!_dataContainer)
                _dataContainer = new GameObject("CompanyDataContainer").AddComponent<CompanyInterLevelDataContainer>();

            Container.Bind<CompanyInterLevelDataContainer>()
                .FromInstance(_dataContainer)
                .AsSingle()
                .NonLazy();
        }
    }
}