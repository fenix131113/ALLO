using PlayerSystem.Attack.Data;
using UnityEngine;

namespace PlayerSystem.Attack.Shooting.Data
{
    [CreateAssetMenu(fileName = "New Firearms Data", menuName = "Configs/Weapon/New Firearms Data")]
    public class FirearmsDataSO : WeaponBaseDataSO
    {
        [field: SerializeField] public Bullet BulletPrefab { get; private set; }
        [field: SerializeField] public AmmoType AmmoType { get; private set; }
        [field: SerializeField] public float ShootCooldown { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public int MaxAmmoInClip { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}