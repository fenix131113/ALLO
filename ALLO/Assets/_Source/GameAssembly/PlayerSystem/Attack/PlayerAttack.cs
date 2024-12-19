using EntityDrawers;
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
        private MutantDrawer _mutantDrawer;

        [Inject]
        public void Construct(PlayerMutation playerMutation, PlayerShoot playerShoot,
            PlayerWeaponsData playerWeaponsData, PlayerMelee playerMelee, MutantDrawer mutantDrawer)
        {
            _playerMutation = playerMutation;
            _playerShoot = playerShoot;
            _playerMelee = playerMelee;
            _playerWeaponsData = playerWeaponsData;
            _mutantDrawer = mutantDrawer;
        }

        public void Attack()
        {
            if (!CurrentWeapon)
                return;

            if (_playerMutation.CurrentPlayer != _playerMutation.DefaultPlayer)
            {
                _mutantDrawer.Attack();
                return;
            }

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