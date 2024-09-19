using GameMenuSystem;
using LevelSystem;
using PlayerSystem;
using PlayerSystem.Data;
using PlayerSystem.Shooting;
using UnityEngine;
using Zenject;

namespace Core
{
	public class GameInstaller : MonoInstaller
	{
		[SerializeField] private Player player;
		[SerializeField] private LevelInitializer levelInitializer;
		[SerializeField] private PlayerMutation playerMutation;
		[SerializeField] private GameMenu gameMenu;
		[SerializeField] private PlayerShoot playerShoot;
		
		[SerializeField] private PlayerMovementConfig playerConfig;
		[SerializeField] private PlayerMouseTargeting playerMouseTargeting;
		
		public override void InstallBindings()
		{
			BindLevelSystem();
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
		
		private void BindLevelSystem()
		{
			Container.Bind<LevelInitializer>()
				.FromInstance(levelInitializer)
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
				.FromInstance(playerMutation)
				.AsSingle();

			Container.Bind<PlayerMouseTargeting>()
				.FromInstance(playerMouseTargeting)
				.AsSingle();

			Container.Bind<PlayerShoot>()
				.FromInstance(playerShoot)
				.AsSingle()
				.NonLazy();
		}
	}
}
