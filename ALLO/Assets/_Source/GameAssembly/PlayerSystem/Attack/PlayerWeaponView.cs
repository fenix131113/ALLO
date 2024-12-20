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
        [SerializeField] private Transform leftArm;
        [SerializeField] private Animator handsAnimator;

        private GameObject _activatedWeapon;
        private PlayerWeaponsData _playerWeaponData;
        private Vector3 _startLeftArmPosition;

        [Inject]
        private void Construct(PlayerWeaponsData playerWeaponData) => _playerWeaponData = playerWeaponData;

        private void Awake()
        {
            Bind();
            _startLeftArmPosition = leftArm.position;
        }

        private void OnDestroy() => Expose();

        public WeaponItemData GetWeaponGFX(WeaponBaseDataSO weaponData) =>
            weaponsGFX.FirstOrDefault(weapon => weapon.Weapon == weaponData);

        public void ActivateWeapon()
        {
            if(!_playerWeaponData.CurrentWeapon)
                return;
            
            var weaponGFX = GetWeaponGFX(_playerWeaponData.CurrentWeapon);

            if (!weaponGFX.WeaponGFX)
                throw new ArgumentException("This weapon GFX doesn't assigned!");

            if (_playerWeaponData.CurrentWeapon.WeaponType != WeaponType.FIREARMS)
            {
                handsAnimator.enabled = true;
                leftArm.localPosition = _startLeftArmPosition;
            }
            else
            {
                handsAnimator.enabled = false;
                leftArm.localPosition = weaponGFX.LeftArmPosition;
            }

            _activatedWeapon?.SetActive(false);
            _activatedWeapon = weaponGFX.WeaponGFX;
            _activatedWeapon.SetActive(true);
        }

        private void Bind() => _playerWeaponData.OnWeaponChanged += ActivateWeapon;

        private void Expose() => _playerWeaponData.OnWeaponChanged -= ActivateWeapon;

        [Serializable]
        public class WeaponItemData
        {
            [field: SerializeField] public WeaponBaseDataSO Weapon { get; private set; }
            [field: SerializeField] public GameObject WeaponGFX { get; private set; }
            [field: SerializeField] public Vector3 LeftArmPosition { get; private set; }
        }
    }
}