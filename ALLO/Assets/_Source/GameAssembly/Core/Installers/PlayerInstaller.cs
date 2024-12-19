using EntityDrawers;
using GameMenuSystem;
using PlayerSystem;
using PlayerSystem.Attack;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Melee;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Throwable;
using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Player player;
        [SerializeField] private GameMenu gameMenu;

        [SerializeField] private PlayerMovementConfig playerConfig;
        [SerializeField] private PlayerMouseTargeting playerMouseTargeting;
        [SerializeField] private StartEquipmentProfileSO startEquipmentProfileSO;

        public override void InstallBindings()
        {
            BindPlayer();
            BindMenuSystem();
        }

        private void BindMenuSystem()
        {
            Container.Bind<GameMenu>()
                .FromInstance(gameMenu)
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

            Container.Bind<PlayerThrowable>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();

            Container.Bind<MutantDrawer>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}