using System.Collections.Generic;
using PlayerSystem.Attack.Data;
using UnityEngine;

namespace PlayerSystem.Attack
{
    [CreateAssetMenu(fileName = "Configs/New Start Equipment", menuName = "Configs/New Start Equipment")]
    public class StartEquipmentProfile : ScriptableObject
    {
        [field: SerializeField] public List<WeaponBaseDataSO> StartWeapons { get; private set; } = new();
        [field: SerializeField] public int StartNineMMAmmo { get; private set; }
    }
}