using System;
using System.Collections.Generic;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Shooting.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem.Attack.Shooting
{
    public class PlayerAmmoContainer
    {
        public event Action OnWeaponAmmoChanged;

        private readonly Dictionary<FirearmsDataSO, int> _playerWeaponAmmo = new();
        private readonly Dictionary<AmmoType, int> _playerStorageAmmo = new();

        [Inject]
        public PlayerAmmoContainer(StartEquipmentProfile startEquipmentProfile)
        {
            TryChangeAmmoInStorage(AmmoType.NINE_MM, startEquipmentProfile.StartNineMMAmmo);
        }

        public void RegisterWeapon(FirearmsDataSO weapon, int ammo = 0)
        {
            _playerWeaponAmmo.TryAdd(weapon, ammo);
        }

        public bool TryChangeAmmoInWeapon(FirearmsDataSO weapon, int count)
        {
            if (_playerWeaponAmmo.TryGetValue(weapon, out _) && _playerWeaponAmmo[weapon] + count >= 0)
            {
                _playerWeaponAmmo[weapon] = Mathf.Clamp(_playerWeaponAmmo[weapon] + count, 0, weapon.MaxAmmoInClip);
                OnWeaponAmmoChanged?.Invoke();
                return true;
            }

            if (count <= 0) return false;

            RegisterWeapon(weapon, count);
            OnWeaponAmmoChanged?.Invoke();
            return true;
        }


        public int GetWeaponAmmo(FirearmsDataSO weapon)
        {
            return _playerWeaponAmmo.GetValueOrDefault(weapon, 0);
        }

        public bool TryChangeAmmoInStorage(AmmoType ammoType, int count)
        {
            if (_playerStorageAmmo.TryGetValue(ammoType, out _) && _playerStorageAmmo[ammoType] + count >= 0)
            {
                _playerStorageAmmo[ammoType] += count;
                OnWeaponAmmoChanged?.Invoke();
                return true;
            }

            if (count <= 0) return false;

            OnWeaponAmmoChanged?.Invoke();
            return _playerStorageAmmo.TryAdd(ammoType, count);
        }

        public int GetAmmoFromStorage(AmmoType ammoType)
        {
            return _playerStorageAmmo.GetValueOrDefault(ammoType, 0);
        }
    }
}