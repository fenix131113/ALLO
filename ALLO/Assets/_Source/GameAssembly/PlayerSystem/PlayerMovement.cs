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

        // Upgrades
        private bool _dashUnlocked;
        private int _movementUpgradedPercentage;

        // Dash Timer
        public bool IsDashTimerProceed { get; private set; }
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

        public void UnlockDash() => _dashUnlocked = true;
        public void AddMovementUpgradePercentage(int percent) => _movementUpgradedPercentage += percent;

        public void MovePlayer(Vector2 movementVector, bool run)
        {
            if (!_canMove)
                return;

            CurrentRunState = run;

            CurrentMovementVector = movementVector.normalized *
                                    (run ? GetUpgradedRunSpeed() : GetUpgradedWalkSpeed());
            _playerMutation.CurrentPlayer.MovePlayer(CurrentMovementVector, CurrentRunState,
                _playerMutation.CurrentPlayer != _playerMutation.DefaultPlayer);

            return;

            float GetUpgradedRunSpeed() => _playerConfig.PlayerRunSpeed +
                                           _playerConfig.PlayerRunSpeed * ((float)_movementUpgradedPercentage / 100);

            float GetUpgradedWalkSpeed() => _playerConfig.PlayerWalkSpeed +
                                            _playerConfig.PlayerWalkSpeed * ((float)_movementUpgradedPercentage / 100);
        }

        public void Dash(Vector2 dashDirection)
        {
            if (IsDashTimerProceed || IsDashCooldown || !_dashUnlocked)
                return;

            _canMove = false;
            _playerMutation.CurrentPlayer.Rb.velocity = Vector2.zero;
            _playerMutation.CurrentPlayer.Rb.AddForce(dashDirection * _playerConfig.PlayerDashPower);
            _dashTimer = Time.time;
            IsDashTimerProceed = true;
            IsDashCooldown = true;
            _dashCooldownTimer = Time.time;
        }

        // Dashing time. Can't move while this time
        private void DashTimerCheck()
        {
            if (Time.time - _dashTimer < _playerConfig.PlayerDashTime) return;

            _canMove = true;
            IsDashTimerProceed = false;
        }

        private void DashCooldownCheck()
        {
            if (Time.time - _dashCooldownTimer < _playerConfig.PlayerDashCooldown) return;

            IsDashCooldown = false;
        }

        public void Tick()
        {
            if (IsDashTimerProceed)
                DashTimerCheck();

            if (IsDashCooldown)
                DashCooldownCheck();
        }
    }
}