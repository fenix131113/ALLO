using System;
using System.Collections;
using PlayerSystem.Shooting.Data;
using UnityEngine;

namespace PlayerSystem.Shooting
{
	public class PlayerShoot : MonoBehaviour //TODO: Replace single weapon logic to multi-system
	{
		[field: SerializeField] public WeaponDataSo CurrentWeapon { get; private set; }
		[field: SerializeField] public int Ammo { get; private set; }
		[field: SerializeField] public int AmmoInClip { get; private set; }

		private bool _canShoot = true;
		private bool _isReloading;
		
		public event Action OnShoot;
		public event Action OnReloaded;
		public event Action OnStartReloading;

		public void Shoot(Transform shootPoint)
		{
			if (!_canShoot || _isReloading || AmmoInClip == 0)
				return;

			Instantiate(CurrentWeapon.BulletPrefab, shootPoint.position,
				shootPoint.rotation); //TODO: Change to dynamic object pool

			ChangeAmmoInClip(-1);

			StartCoroutine(ShootCooldown());
			
			OnShoot?.Invoke();
		}

		public void Reload()
		{
			if (Ammo > 0 && AmmoInClip < CurrentWeapon.MaxAmmoInClip)
				StartCoroutine(ReloadCooldown());
		}

		private void ChangeAmmo(int ammoValue)
		{
			Ammo += ammoValue;
		}

		private void ChangeAmmoInClip(int ammoInClipValue)
		{
			if(AmmoInClip == 0)
				return;
			
			AmmoInClip += ammoInClipValue;
		}

		private IEnumerator ShootCooldown()
		{
			_canShoot = false;

			yield return new WaitForSeconds(CurrentWeapon.ShootCooldown);

			_canShoot = true;
		}

		private IEnumerator ReloadCooldown()
		{
			_canShoot = false;
			_isReloading = true;
			OnStartReloading?.Invoke();

			yield return new WaitForSeconds(CurrentWeapon.ReloadTime);

			if (Ammo > CurrentWeapon.MaxAmmoInClip)
			{
				AmmoInClip = CurrentWeapon.MaxAmmoInClip;
				ChangeAmmo(-CurrentWeapon.MaxAmmoInClip);
			}
			else
			{
				AmmoInClip = Ammo;
				ChangeAmmo(-Ammo);
			}

			_canShoot = true;
			_isReloading = false;

			OnReloaded?.Invoke();
		}
	}
}