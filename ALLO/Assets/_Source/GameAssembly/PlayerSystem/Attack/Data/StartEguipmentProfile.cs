using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem.Attack.Data
{
    [CreateAssetMenu(fileName = "Configs/New Start Equipment", menuName = "Configs/New Start Equipment")]
    public class StartEquipmentProfileSO : ScriptableObject
    {
        [field: SerializeField] public List<WeaponBaseDataSO> StartWeapons { get; private set; } = new();
        [field: SerializeField] public int StartNineMMAmmo { get; private set; }
    }
}