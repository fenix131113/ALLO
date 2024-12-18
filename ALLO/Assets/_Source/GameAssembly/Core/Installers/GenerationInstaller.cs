using LevelGenerationSystem;
using LevelGenerationSystem.Data;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GenerationInstaller : MonoInstaller
    {
        [SerializeField] private AstarPath pathFinder;

        [SerializeField] private GenerationSettingsSO generationSettingsSO;

        public override void InstallBindings()
        {
            BindLevelGeneration();
            BindUpgrades();
        }

        private void BindUpgrades()
        {
            Container.Bind<PlayerUpgrade>()
                .AsSingle()
                .NonLazy();
        }

        private void BindLevelGeneration()
        {
            Container.BindInterfacesAndSelfTo<LevelGeneration>()
                .AsSingle()
                .NonLazy();

            Container.Bind<GenerationSettingsSO>()
                .FromInstance(generationSettingsSO)
                .AsSingle()
                .NonLazy();

            Container.Bind<AstarPath>()
                .FromInstance(pathFinder)
                .AsSingle()
                .NonLazy();
        }
    }
}