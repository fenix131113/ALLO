using System.Collections.Generic;
using System.Linq;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Shooting.Data;
using PlayerSystem.Data;
using UnityEngine;

namespace Company
{
    public class CompanyInterLevelDataContainer : MonoBehaviour
    {
        public List<WeaponBaseDataSO> Weapons { get; private set; } = new();
        public Dictionary<UpgradeType, int> CurrentUpgradesLevels { get; private set; } = new();
        public Dictionary<FirearmsDataSO, int> PlayerWeaponAmmo { get; private set; }
        public Dictionary<AmmoType, int> PlayerStorageAmmo { get; private set; }
        public int CompleteLevels { get; private set; }
        public int DefaultPlayerHealth { get; private set; }
        public int MutatedPlayerHealth { get; private set; }
        public int GrenadesCount { get; private set; }

        private void Awake() => DontDestroyOnLoad(gameObject);

        public void SetWeaponsData(List<WeaponBaseDataSO> weapons) => Weapons = weapons;

        public void IncreaseCompleteLevels() => CompleteLevels++;

        public void SetCurrentUpgradesLevels(IReadOnlyDictionary<UpgradeType, int> upgrades) =>
            CurrentUpgradesLevels = upgrades.ToDictionary(key => key.Key, value => value.Value);

        public void SetDefaultPlayerHealth(int health) => DefaultPlayerHealth = health;

        public void SetMutatedPlayerHealth(int health) => MutatedPlayerHealth = health;
        
        public void SetGrenadesCount(int count) => GrenadesCount = count;

        public void SetWeaponAmmoData(Dictionary<FirearmsDataSO, int> playerWeaponAmmo, Dictionary<AmmoType, int> playerStorageAmmo)
        {
            PlayerWeaponAmmo = playerWeaponAmmo;
            PlayerStorageAmmo = playerStorageAmmo;
        }

        public void ClearSaves()
        {
            Weapons.Clear();
            CurrentUpgradesLevels.Clear();
            CompleteLevels = 0;
        }
    }
}