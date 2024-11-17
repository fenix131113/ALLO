using System;
using System.Collections.Generic;
using System.Linq;
using PlayerSystem.Attack.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem.Attack
{
    public class PlayerWeaponView : MonoBehaviour
    {
        [SerializeField] private List<WeaponItemData> weaponsGFX = new();

        private GameObject _activatedWeapon;
        private PlayerWeaponsData _playerWeaponData;

        [Inject]
        private void Construct(PlayerWeaponsData playerWeaponData) => _playerWeaponData = playerWeaponData;

        private void Awake() => Bind();

        private void OnDestroy() => Expose();

        public GameObject GetWeaponGFX(WeaponBaseDataSO weaponData) =>
            weaponsGFX.FirstOrDefault(weapon => weapon.Weapon == weaponData)?.WeaponGFX;

        public void ActivateWeapon()
        {
            var weaponGFX = GetWeaponGFX(_playerWeaponData.CurrentWeapon);

            if (!weaponGFX)
#if UNITY_EDITOR
                Debug.LogWarning("This weapon GFX doesn't assigned!");
#endif
            else
            {
                _activatedWeapon?.SetActive(false);
                _activatedWeapon = weaponGFX;
                _activatedWeapon.SetActive(true);
            }
        }

        private void Bind() => _playerWeaponData.OnWeaponChanged += ActivateWeapon;

        private void Expose() => _playerWeaponData.OnWeaponChanged -= ActivateWeapon;

        [Serializable]
        private class WeaponItemData
        {
            [field: SerializeField] public WeaponBaseDataSO Weapon { get; private set; }
            [field: SerializeField] public GameObject WeaponGFX { get; private set; }
        }
    }
}