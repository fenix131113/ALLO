using System.Collections.Generic;
using Company;
using PlayerSystem;
using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class CompanyInstaller : MonoInstaller
    {
        [SerializeField] private AllUpgradesConfigSO upgradesData;
        
        private CompanyInterLevelDataContainer _dataContainer;

        public override void InstallBindings()
        {
            BindCompany();
            BindUpgrades();
        }
        
        private void BindUpgrades()
        {
            Container.Bind<PlayerUpgradesGenerator>()
                .AsSingle()
                .NonLazy();

            Container.Bind<AllUpgradesConfigSO>()
                .FromInstance(upgradesData)
                .AsSingle()
                .NonLazy();
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