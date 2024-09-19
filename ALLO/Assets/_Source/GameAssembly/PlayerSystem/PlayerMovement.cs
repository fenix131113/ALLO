using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMovement : ITickable
	{
		private readonly PlayerMovementConfig _playerConfig;
		private readonly PlayerMutation _playerMutation;

		private bool _canMove = true;

		// Dash Timer
		private bool _isDashTimerProceed;
		private float _dashTimer;
		
		//Dash Cooldown
		private float _dashCooldownTimer;
		public bool IsDashCooldown { get; private set; }

		//Public movement data
		public Vector2 CurrentMovementVector { get; private set; }
		public bool CurrentRunState { get; private set; }
		public float DashCooldownProgress => (Time.time - _dashCooldownTimer) / _playerConfig.PlayerDashCooldown;

		[Inject]
		public PlayerMovement(PlayerMovementConfig playerConfig, PlayerMutation playerMutation)
		{
			_playerConfig = playerConfig;
			_playerMutation = playerMutation;
		}

		public void MovePlayer(Vector2 movementVector, bool run)
		{
			if (!_canMove)
				return;

			CurrentRunState = run;
			
			_playerMutation.CurrentPlayer.BodyDrawer.SetRunState(run);
			
			CurrentMovementVector = movementVector * (run ? _playerConfig.PlayerRunSpeed : _playerConfig.PlayerWalkSpeed);
			_playerMutation.CurrentPlayer.Rb.velocity = CurrentMovementVector;

		}

		public void Dash(Vector2 dashDirection)
		{
			if (_isDashTimerProceed || IsDashCooldown)
				return;

			_canMove = false;
			_playerMutation.CurrentPlayer.Rb.velocity = Vector2.zero;
			_playerMutation.CurrentPlayer.Rb.AddForce(dashDirection * _playerConfig.PlayerDashPower);
			_dashTimer = Time.time;
			_isDashTimerProceed = true;
			IsDashCooldown = true;
			_dashCooldownTimer = Time.time;
		}

		// Dashing time. Can't move while this time
		private void DashTimerCheck()
		{
			if (Time.time - _dashTimer < _playerConfig.PlayerDashTime) return;

			_canMove = true;
			_isDashTimerProceed = false;
		}

		private void DashCooldownCheck()
		{
			if(Time.time - _dashCooldownTimer < _playerConfig.PlayerDashCooldown) return;

			IsDashCooldown = false;
		}

		public void Tick()
		{
			if (_isDashTimerProceed)
				DashTimerCheck();
			
			if(IsDashCooldown)
				DashCooldownCheck();
		}
	}
}