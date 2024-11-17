using System;
using System.Collections.Generic;
using System.Linq;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Shooting.Data;
using Zenject;

// ReSharper disable InvertIf

namespace PlayerSystem.Attack
{
    public class PlayerWeaponsData : IInitializable
    {
        public List<WeaponBaseDataSO> Weapons { get; private set; } = new();
        public WeaponBaseDataSO CurrentWeapon => Weapons[_selectedIndex];

        private int _selectedIndex;
        
        public event Action OnWeaponChanged;

        [Inject]
        public PlayerWeaponsData(StartEquipmentProfile startEquipmentProfile, PlayerAmmoContainer playerAmmoContainer)
        {
            foreach (var weapon in startEquipmentProfile.StartWeapons.Where(weapon => !Weapons.Contains(weapon)))
            {
                TryAddWeapon(weapon);

                if (weapon.WeaponType == WeaponType.FIREARMS)
                {
                    var firearm = weapon.GetCurrentWeaponSO<FirearmsDataSO>();
                    playerAmmoContainer.RegisterWeapon(firearm, firearm.MaxAmmoInClip);
                }
            }
        }

        public void Initialize()
        {
            SelectWeapon(0);
        }
        
        public bool TryAddWeapon(WeaponBaseDataSO weapon)
        {
            if (!Weapons.Contains(weapon))
            {
                Weapons.Add(weapon);
                return true;
            }

            return false;
        }

        public void SelectWeapon(WeaponBaseDataSO weapon)
        {
            for (var i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i] == weapon)
                {
                    _selectedIndex = i;
                    break;
                }
            }
            
            OnWeaponChanged?.Invoke();
        }

        private void SelectWeapon(int index)
        {
            _selectedIndex = index;            
            
            OnWeaponChanged?.Invoke();
        }

        public void ScrollWeapon(float scrollAmount)
        {
            if (scrollAmount > 0)
            {
                if(_selectedIndex + 1 >= Weapons.Count)
                    SelectWeapon(0);
                else
                    SelectWeapon(_selectedIndex + 1);
            }
            else
            {
                if(_selectedIndex - 1 < 0)
                    SelectWeapon(Weapons.Count - 1);
                else
                    SelectWeapon(_selectedIndex - 1);
            }
        }
    }
}