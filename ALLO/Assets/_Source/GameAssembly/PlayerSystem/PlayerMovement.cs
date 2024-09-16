using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMovement
	{
		private PlayerMovementConfig _playerConfig;
		private Player _player;

		[Inject]
		public PlayerMovement(PlayerMovementConfig playerConfig, Player player)
		{
			_playerConfig = playerConfig;
			_player = player;
		}

		public void MovePlayer(Vector2 movementVector)
		{
			_player.Rb.velocity = movementVector * _playerConfig.PlayerWalkSpeed;
		}
	}
}
