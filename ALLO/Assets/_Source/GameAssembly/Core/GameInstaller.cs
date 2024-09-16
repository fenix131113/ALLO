using PlayerSystem;
using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace Core
{
	public class GameInstaller : MonoInstaller
	{
		[SerializeField] private Player player;
		
		[SerializeField] private PlayerMovementConfig playerConfig;
		
		public override void InstallBindings()
		{
			BindPlayer();
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
		}
	}
}
