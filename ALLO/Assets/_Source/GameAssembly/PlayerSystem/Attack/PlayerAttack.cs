using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Melee;
using PlayerSystem.Attack.Shooting;
using Zenject;

namespace PlayerSystem.Attack
{
    public class PlayerAttack
    {
        public WeaponBaseDataSO CurrentWeapon => _playerWeaponsData.CurrentWeapon;
        
        private PlayerMutation _playerMutation;
        private PlayerShoot _playerShoot;
        private PlayerMelee _playerMelee;
        private PlayerWeaponsData _playerWeaponsData;

        [Inject]
        public void Construct(PlayerMutation playerMutation, PlayerShoot playerShoot,
            PlayerWeaponsData playerWeaponsData, PlayerMelee playerMelee)
        {
            _playerMutation = playerMutation;
            _playerShoot = playerShoot;
            _playerMelee = playerMelee;
            _playerWeaponsData = playerWeaponsData;
        }

        public void Attack()
        {
            if (!CurrentWeapon)
                return;

            if (CurrentWeapon.WeaponType == WeaponType.FIREARMS)
                _playerShoot.Shoot(_playerMutation.CurrentPlayer.ShootPoint);
            else
                _playerMelee.Hit();
        }

        public void ReloadRequest()
        {
            if (!CurrentWeapon)
                return;

            if (CurrentWeapon.WeaponType == WeaponType.FIREARMS)
                _playerShoot.Reload();
        }
    }
}