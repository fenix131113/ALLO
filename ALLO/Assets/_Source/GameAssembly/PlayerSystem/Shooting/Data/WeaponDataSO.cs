using UnityEngine;

namespace PlayerSystem.Shooting.Data
{
	[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Configs/New Weapon Data")]
	public class WeaponDataSo : ScriptableObject
	{
		[field: SerializeField] public GameObject BulletPrefab { get; private set; }
		[field: SerializeField] public float ShootCooldown { get; private set; }
		[field: SerializeField] public float ReloadTime { get; private set; }
		[field: SerializeField] public int MaxAmmoInClip { get; private set; }
	}
}