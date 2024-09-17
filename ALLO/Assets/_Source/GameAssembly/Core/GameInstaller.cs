using System.Collections.Generic;
using LevelSystem;
using PlayerSystem;
using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace Core
{
	public class GameInstaller : MonoInstaller
	{
		[SerializeField] private Player player;
		[SerializeField] private LevelInitializer levelInitializer;
		[SerializeField] private PlayerMutation playerMutation;
		
		[SerializeField] private PlayerMovementConfig playerConfig;
		
		public override void InstallBindings()
		{
			BindLevelSystem();
			BindPlayer();
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
			Container.Bind<PlayerMovement>()
				.AsSingle()
				.NonLazy();
			
			Container.BindInterfacesTo<PlayerInputHandler>()
				.AsSingle()
				.NonLazy();

			Container.Bind<PlayerMovementConfig>()
				.FromInstance(playerConfig)
				.AsSingle()
				.NonLazy();

			Container.Bind<Player>()
				.FromInstance(player)
				.AsSingle()
				.NonLazy();

			Container.Bind<PlayerMutation>()
				.FromInstance(playerMutation)
				.AsSingle()
				.NonLazy();
		}
	}
}
