using PlayerSystem.Attack.Data;
using UnityEngine;

namespace PlayerSystem.Attack.Melee.Data
{
    [CreateAssetMenu(fileName = "New Firearms Data", menuName = "Configs/Weapon/New Melee Data")]
    public class MeleeDataSO : WeaponBaseDataSO
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
    }
}