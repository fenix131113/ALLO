using System;
using System.Collections;
using PlayerSystem.Shooting.Data;
using UnityEngine;

namespace PlayerSystem.Shooting
{
	public class PlayerShoot : MonoBehaviour //TODO: Replace single weapon logic to multi-system
	{
		[SerializeField] private WeaponDataSo currentWeapon;
		[field: SerializeField] public int Ammo { get; private set; }
		[field: SerializeField] public int AmmoInClip { get; private set; }

		private bool _canShoot = true;
		private bool _isReloading;
		
		public event Action OnShoot;
		public event Action OnReloaded;

		public void Shoot(Transform shootPoint)
		{
			if (!_canShoot || _isReloading || AmmoInClip == 0)
				return;

			Instantiate(currentWeapon.BulletPrefab, shootPoint.position,
				shootPoint.rotation); //TODO: Change to dynamic object pool

			ChangeAmmoInClip(-1);

			StartCoroutine(ShootCooldown());
			
			OnShoot?.Invoke();
		}

		public void Reload()
		{
			if (Ammo > 0 && AmmoInClip < currentWeapon.MaxAmmoInClip)
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

			yield return new WaitForSeconds(currentWeapon.ShootCooldown);

			_canShoot = true;
		}

		private IEnumerator ReloadCooldown()
		{
			_canShoot = false;
			_isReloading = true;

			yield return new WaitForSeconds(currentWeapon.ReloadTime);

			if (Ammo > currentWeapon.MaxAmmoInClip)
			{
				AmmoInClip = currentWeapon.MaxAmmoInClip;
				ChangeAmmo(-currentWeapon.MaxAmmoInClip);
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