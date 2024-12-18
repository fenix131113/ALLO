using UnityEngine;

namespace PlayerSystem.Data
{
    [CreateAssetMenu(menuName = "Configs/Player/Upgrade", fileName = "Upgrade Data")]
    public class UpgradesDataSO : ScriptableObject
    {
        [field: SerializeField] public string UpgradeName { get; private set; }
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
        [field: SerializeField] public int Weight { get; private set; }
        [field: SerializeField] public int UpgradeValue { get; private set; }
        [field: SerializeField] public int MaxLevel { get; private set; }
    }
}