using PlayerSystem.Shooting.Data;
using UnityEngine;

namespace PlayerSystem.Shooting
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private WeaponDataSo currentWeapon;

        public void Shoot(Transform shootPoint)
        {
            Instantiate(currentWeapon.BulletPrefab, shootPoint.position, shootPoint.rotation); //TODO: Change to dynamic object pool
        }
    }
}
