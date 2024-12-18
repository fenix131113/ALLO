using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem.Data
{
    [CreateAssetMenu(fileName = "New All Upgrades Config", menuName = "Configs/Player/All Upgrades Config")]
    public class AllUpgradesConfigSO : ScriptableObject
    {
        [field: SerializeField] public List<UpgradesDataSO> AllUpgrades { get; set; }
    }
}