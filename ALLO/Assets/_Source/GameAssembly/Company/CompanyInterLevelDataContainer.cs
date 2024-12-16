using System.Collections.Generic;
using PlayerSystem.Attack.Data;
using UnityEngine;

namespace Company
{
    public class CompanyInterLevelDataContainer : MonoBehaviour
    {
        public List<WeaponBaseDataSO> Weapons { get; private set; } = new();
        public int CompleteLevels { get; private set; }
        
        private void Awake() => DontDestroyOnLoad(gameObject);

        public void SetWeaponsData(List<WeaponBaseDataSO> weapons) => Weapons = weapons;

        public void IncreaseCompleteLevels() => CompleteLevels++;
        
        public void ClearSaves() => Weapons.Clear();
    }
}