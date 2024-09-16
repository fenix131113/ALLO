using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerInputHandler : ITickable
    {
        private readonly PlayerMovement _playerMovement;

        [Inject]
        public PlayerInputHandler(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }
        
        public void Tick()
        {
            ReadMovementInput();
        }

        private void ReadMovementInput()
        {
            Vector2 movementInput = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _playerMovement.MovePlayer(movementInput);
        }
    }
}
