using System;
using System.Collections;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Shooting.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem.Attack.Shooting
{
    public class PlayerShoot : MonoBehaviour
    {
        public FirearmsDataSO CurrentFirearm => _playerAttack.CurrentWeapon.WeaponType == WeaponType.FIREARMS
            ? _playerAttack.CurrentWeapon.GetCurrentWeaponSO<FirearmsDataSO>()
            : null;

        private PlayerAttack _playerAttack;
        private PlayerAmmoContainer _ammoContainer;
        private PlayerWeaponsData _playerWeaponsData;
        private bool _canShoot = true;
        private bool _isReloading;

        public event Action OnShoot;
        public event Action OnReloaded;
        public event Action OnStartReloading;

        [Inject]
        private void Construct(PlayerAttack playerAttack, PlayerAmmoContainer ammoContainer,
            PlayerWeaponsData playerWeaponsData)
        {
            _playerAttack = playerAttack;
            _ammoContainer = ammoContainer;
            _playerWeaponsData = playerWeaponsData;
        }

        private void Awake() => Bind();

        private void OnDestroy() => Expose();

        private void Bind() => _playerWeaponsData.OnWeaponChanged += OnWeaponChanged;

        private void Expose() => _playerWeaponsData.OnWeaponChanged -= OnWeaponChanged;

        private void OnWeaponChanged()
        {
            StopAllCoroutines();
            _canShoot = true;
            _isReloading = false;
        }

        public void Shoot(Transform shootPoint)
        {
            if (!CurrentFirearm)
                return;

            if (_ammoContainer.GetWeaponAmmo(CurrentFirearm) == 0 && !_isReloading &&
                _ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType) > 0)
                Reload();

            if (!_canShoot || _isReloading || _ammoContainer.GetWeaponAmmo(CurrentFirearm) == 0)
                return;

            Instantiate(CurrentFirearm.BulletPrefab, shootPoint.position,
                shootPoint.rotation); //TODO: Change to dynamic object pool

            StartCoroutine(ShootCooldown());

            TryChangeAmmoInClip(-1);

            OnShoot?.Invoke();
        }

        public void Reload()
        {
            if (!CurrentFirearm)
                return;

            if (!_isReloading && _ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType) > 0 &&
                _ammoContainer.GetWeaponAmmo(CurrentFirearm) < CurrentFirearm.MaxAmmoInClip)
                StartCoroutine(ReloadCooldown());
        }

        private bool TryChangeAmmoInClip(int ammoInClipValue) =>
            _ammoContainer.GetWeaponAmmo(CurrentFirearm) != 0 &&
            _ammoContainer.TryChangeAmmoInWeapon(CurrentFirearm,
                ammoInClipValue);

        private IEnumerator ShootCooldown()
        {
            _canShoot = false;

            yield return new WaitForSeconds(CurrentFirearm.ShootCooldown);

            _canShoot = true;
        }

        private IEnumerator ReloadCooldown()
        {
            if (!CurrentFirearm)
                yield return null;

            _canShoot = false;
            _isReloading = true;
            OnStartReloading?.Invoke();

            yield return new WaitForSeconds(CurrentFirearm.ReloadTime);

            if (_ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType) >=
                CurrentFirearm.MaxAmmoInClip - _ammoContainer.GetWeaponAmmo(CurrentFirearm))
            {
                var temp = CurrentFirearm.MaxAmmoInClip - _ammoContainer.GetWeaponAmmo(CurrentFirearm);

                if (_ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType) < temp)
                    temp = _ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType);

                _ammoContainer.TryChangeAmmoInWeapon(CurrentFirearm, temp);
                _ammoContainer.TryChangeAmmoInStorage(CurrentFirearm.AmmoType, -temp);
            }
            else
            {
                _ammoContainer.TryChangeAmmoInWeapon(CurrentFirearm,
                    _ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType));
                _ammoContainer.TryChangeAmmoInStorage(CurrentFirearm.AmmoType,
                    -_ammoContainer.GetAmmoFromStorage(CurrentFirearm.AmmoType));
            }

            _canShoot = true;
            _isReloading = false;

            OnReloaded?.Invoke();
        }
    }
}