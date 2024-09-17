using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMovement
	{
		private readonly PlayerMovementConfig _playerConfig;
		private readonly PlayerMutation _playerMutation;

		[Inject]
		public PlayerMovement(PlayerMovementConfig playerConfig, PlayerMutation playerMutation)
		{
			_playerConfig = playerConfig;
			_playerMutation = playerMutation;
		}

		public void MovePlayer(Vector2 movementVector, bool run)
		{
			_playerMutation.CurrentPlayer.Rb.velocity = movementVector * (run ? _playerConfig.PlayerRunSpeed : _playerConfig.PlayerWalkSpeed);
		}
	}
}
