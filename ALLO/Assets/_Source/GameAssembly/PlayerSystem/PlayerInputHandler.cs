using GameMenuSystem;
using PlayerSystem.Attack;
using PlayerSystem.Attack.Shooting;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerInputHandler : ITickable, IInitializable
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerMutation _playerMutation;
        private readonly GameMenu _gameMenu;
        private readonly PlayerAttack _playerAttack;
        private readonly PlayerWeaponsData _playerWeaponsData;

        private bool _isReadPaused;

        [Inject]
        public PlayerInputHandler(PlayerMovement playerMovement, PlayerMutation playerMutation, GameMenu gameMenu,
            PlayerAttack playerAttack, PlayerWeaponsData playerWeaponsData)
        {
            _playerMovement = playerMovement;
            _playerMutation = playerMutation;
            _gameMenu = gameMenu;
            _playerAttack = playerAttack;
            _playerWeaponsData = playerWeaponsData;
        }

        public void Tick()
        {
            ReadGameMenuInput();

            if (_isReadPaused)
                return;

            ReadMovementInput();
            //ReadMutationInput();
            ReadDashMovementInput();
            ReadAttackInput();
            ReadReloadInput();
            ReadScrollInput();
        }

        public void Initialize()
        {
            _gameMenu.OnContinue += Unpause;
        }

        private void ReadScrollInput()
        {
            if (Input.mouseScrollDelta.y != 0)
                _playerWeaponsData.ScrollWeapon(Input.mouseScrollDelta.y);
        }

        private void Unpause() => _isReadPaused = false;

        private void ReadGameMenuInput()
        {
            if (!Input.GetKeyDown(KeyCode.Escape) || _playerMutation.CurrentPlayer.Health == 0) return;

            _isReadPaused = !_isReadPaused;
            _gameMenu.SwitchMenu(_isReadPaused);
        }

        private void ReadReloadInput()
        {
            if (!Input.GetKeyDown(KeyCode.R))
                return;

            _playerAttack.ReloadRequest();
        }

        private void ReadAttackInput()
        {
            if (Input.GetMouseButtonDown(0))
                _playerAttack.Attack();
        }

        private void ReadDashMovementInput()
        {
            var dashVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space))
                _playerMovement.Dash(dashVector);
        }

        private void ReadMovementInput()
        {
            Vector2 movementInput = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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