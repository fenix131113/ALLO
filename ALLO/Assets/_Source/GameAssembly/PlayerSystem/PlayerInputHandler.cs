using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerInputHandler : ITickable
	{
		private readonly PlayerMovement _playerMovement;
		private readonly PlayerMutation _playerMutation;

		[Inject]
		public PlayerInputHandler(PlayerMovement playerMovement, PlayerMutation playerMutation)
		{
			_playerMovement = playerMovement;
			_playerMutation = playerMutation;
		}

		public void Tick()
		{
			ReadMovementInput();
			ReadMutationInput();
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