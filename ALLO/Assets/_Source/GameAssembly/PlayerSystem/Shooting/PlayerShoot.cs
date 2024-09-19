using System;
using System.Collections;
using PlayerSystem.Shooting.Data;
using UnityEngine;

namespace PlayerSystem.Shooting
{
	public class PlayerShoot : MonoBehaviour //TODO: Replace single weapon logic to multi-system
	{
		[SerializeField] private WeaponDataSo currentWeapon;
		[SerializeField] private int ammo;
		[SerializeField] private int ammoInClip;

		private bool _canShoot = true;
		private bool _isReloading;
		
		public event Action OnAmmoChanged;

		public void Shoot(Transform shootPoint)
		{
			if (!_canShoot || _isReloading || ammoInClip == 0)
				return;

			Instantiate(currentWeapon.BulletPrefab, shootPoint.position,
				shootPoint.rotation); //TODO: Change to dynamic object pool

			ChangeAmmoInClip(-1);

			StartCoroutine(ShootCooldown());
		}

		public void Reload()
		{
			if (ammo > 0 && ammoInClip < currentWeapon.MaxAmmoInClip)
				StartCoroutine(ReloadCooldown());
		}

		private void ChangeAmmo(int ammoValue)
		{
			ammo += ammoValue;
			OnAmmoChanged?.Invoke();
		}

		private void ChangeAmmoInClip(int ammoInClipValue)
		{
			if(ammoInClip == 0)
				return;
			
			ammoInClip += ammoInClipValue;
			OnAmmoChanged?.Invoke();
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

			if (ammo > currentWeapon.MaxAmmoInClip)
			{
				ammoInClip = currentWeapon.MaxAmmoInClip;
				ChangeAmmo(-currentWeapon.MaxAmmoInClip);
			}
			else
			{
				ammoInClip = ammo;
				ChangeAmmo(-ammo);
			}

			_canShoot = true;
			_isReloading = false;
		}
	}
}