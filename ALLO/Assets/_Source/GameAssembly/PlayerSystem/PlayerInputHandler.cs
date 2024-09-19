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
		}
		
		public void Initialize()
		{
			_gameMenu.OnContinue += Unpause;
		}
		
		private void Unpause() => _isReadPaused = false;

		private void ReadGameMenuInput()
		{
			if (!Input.GetKeyDown(KeyCode.Escape)) return;
			
			_isReadPaused = !_isReadPaused;
			_gameMenu.SwitchMenu(_isReadPaused);
		}

		private void ReadShootInput()
		{
			if(Input.GetMouseButtonDown(0))
				_playerShoot.Shoot(_playerMutation.CurrentPlayer.ShootPoint);
		}

		private void ReadDashMovementInput()
		{
			var dashVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if (Input.GetKeyDown(KeyCode.E))
				_playerMovement.Dash(dashVector);
		}

		private void ReadMovementInput()
		{
			Vector2 movementInput = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			bool run = Input.GetKey(KeyCode.LeftShift);

			_playerMovement.MovePlayer(movementInput, run);
		}

		private void ReadMutationInput()
		{
			if (Input.GetKeyDown(KeyCode.F))
				_playerMutation.SwitchMutate();
		}
	}
}