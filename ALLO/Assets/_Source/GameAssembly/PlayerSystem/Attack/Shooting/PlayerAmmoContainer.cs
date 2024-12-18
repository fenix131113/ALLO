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
        public IReadOnlyDictionary<FirearmsDataSO, int> PlayerWeaponAmmo => _playerWeaponAmmo;
        public IReadOnlyDictionary<AmmoType, int> PlayerStorageAmmo => _playerStorageAmmo; 
        
        public event Action OnWeaponAmmoChanged;

        private Dictionary<FirearmsDataSO, int> _playerWeaponAmmo = new();
        private Dictionary<AmmoType, int> _playerStorageAmmo = new();

        [Inject]
        public PlayerAmmoContainer(StartEquipmentProfileSO startEquipmentProfileSO)
        {
            TryChangeAmmoInStorage(AmmoType.NINE_MM, startEquipmentProfileSO.StartNineMMAmmo);
        }

        public void LoadData(Dictionary<FirearmsDataSO, int> playerWeaponAmmo, Dictionary<AmmoType, int> playerStorageAmmo)
        {
            _playerWeaponAmmo = playerWeaponAmmo;
            _playerStorageAmmo = playerStorageAmmo;
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


        public int GetWeaponAmmo(FirearmsDataSO weapon) => _playerWeaponAmmo.GetValueOrDefault(weapon, 0);

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