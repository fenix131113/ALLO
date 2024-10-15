using GameMenuSystem;
using PlayerSystem.Shooting;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerInputHandler : ITickable, IInitializable
	{
		private readonly PlayerMovement _playerMovement;
		private readonly PlayerMutation _playerMutation;
		private readonly GameMenu _gameMenu;
		private readonly PlayerShoot _playerShoot;

		private bool _isReadPaused;

		[Inject]
		public PlayerInputHandler(PlayerMovement playerMovement, PlayerMutation playerMutation, GameMenu gameMenu, PlayerShoot playerShoot)
		{
			_playerMovement = playerMovement;
			_playerMutation = playerMutation;
			_gameMenu = gameMenu;
			_playerShoot = playerShoot;
		}
		
		public void Tick()
		{
			ReadGameMenuInput();
			
			if (_isReadPaused)
				return;
			
			ReadMovementInput();
			//ReadMutationInput();
			ReadDashMovementInput();
			ReadShootInput();
			ReadReloadInput();
		}
		
		public void Initialize()
		{
			_gameMenu.OnContinue += Unpause;
		}
		
		private void Unpause() => _isReadPaused = false;
		
		private void ReadGameMenuInput()
		{
			if (!Input.GetKeyDown(KeyCode.Escape) || _playerMutation.CurrentPlayer.Health == 0) return;
			
			_isReadPaused = !_isReadPaused;
			_gameMenu.SwitchMenu(_isReadPaused);
		}

		public void ReadReloadInput()
		{
			if (!Input.GetKeyDown(KeyCode.R))
				return;
			
			_playerShoot.Reload();
		}

		private void ReadShootInput()
		{
			if(Input.GetMouseButtonDown(0))
				_playerShoot.Shoot(_playerMutation.CurrentPlayer.ShootPoint);
		}

		private void ReadDashMovementInput()
		{
			var dashVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if (Input.GetKeyDown(KeyCode.Space))
				_playerMovement.Dash(dashVector);
		}

		private void ReadMovementInput()
		{
			Vector2 movementInput = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			var run = Input.GetKey(KeyCode.LeftShift);

			_playerMovement.MovePlayer(movementInput, run);
		}

		private void ReadMutationInput()
		{
			if (Input.GetKeyDown(KeyCode.F))
				_playerMutation.SwitchMutation();
		}
	}
}