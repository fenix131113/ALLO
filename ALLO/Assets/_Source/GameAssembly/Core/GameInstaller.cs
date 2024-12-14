using Company;
using GameMenuSystem;
using LevelGenerationSystem;
using LevelGenerationSystem.Data;
using PlayerSystem;
using PlayerSystem.Attack;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Melee;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Player player;
        [SerializeField] private GameMenu gameMenu;
        [SerializeField] private AstarPath pathFinder;

        [SerializeField] private PlayerMovementConfig playerConfig;
        [SerializeField] private PlayerMouseTargeting playerMouseTargeting;
        [SerializeField] private StartEquipmentProfileSO startEquipmentProfileSO;
        [SerializeField] private GenerationSettingsSO generationSettingsSO;

        public override void InstallBindings()
        {
            //TODO: Delete this name checking
            if (SceneManager.GetActiveScene().name != "Game")
                BindLevelGeneration();
            BindPlayer();
            BindMenuSystem();
            BindCompany();
        }

        private void BindCompany()
        {
            Container.Bind<CompanyInterLevelDataContainer>()
                .AsSingle()
                .NonLazy();
        }

        private void BindMenuSystem()
        {
            Container.Bind<GameMenu>()
                .FromInstance(gameMenu)
                .AsSingle()
                .NonLazy();
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

        private void BindPlayer()
        {
            Container.BindInterfacesAndSelfTo<PlayerMovement>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerInputHandler>()
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerMovementConfig>()
                .FromInstance(playerConfig)
                .AsSingle();

            Container.Bind<Player>()
                .FromInstance(player)
                .AsSingle();

            Container.Bind<PlayerMutation>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<PlayerMouseTargeting>()
                .FromInstance(playerMouseTargeting)
                .AsSingle();

            Container.Bind<PlayerShoot>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerAttack>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerWeaponsData>()
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerAmmoContainer>()
                .AsSingle()
                .NonLazy();

            Container.Bind<StartEquipmentProfileSO>()
                .FromInstance(startEquipmentProfileSO)
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerMelee>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}