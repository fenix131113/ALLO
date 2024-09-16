using PlayerSystem.Data;
using UnityEngine;

namespace PlayerSystem
{
	public class PlayerMovement
	{
		private Rigidbody2D _playerRb;
		private PlayerMovementConfig _playerConfig;

		public PlayerMovement(PlayerMovementConfig playerConfig)
		{
			_playerConfig = playerConfig;
		}

		public void MovePlayer(Vector2 movementVector)
		{
			_playerRb.velocity = movementVector * _playerConfig.PlayerWalkSpeed;
		}
	}
}
