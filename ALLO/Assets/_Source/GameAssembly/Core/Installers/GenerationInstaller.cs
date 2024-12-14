using LevelGenerationSystem;
using LevelGenerationSystem.Data;
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
        }

        private void BindLevelGeneration()
        {
            Container.BindInterfacesTo<LevelGeneration>()
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