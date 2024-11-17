using System.Collections;
using DamageSystem;
using PlayerSystem.Attack.Data;
using PlayerSystem.Attack.Melee.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem.Attack.Melee
{
    public class PlayerMelee : MonoBehaviour
    {
        private static readonly int _hitKey = Animator.StringToHash("Hit");
        private static readonly int _meleeHit = Animator.StringToHash("MeleeHit");

        [SerializeField] private Animator handsAnim;
        [SerializeField] private Animator hitAnim;
        [SerializeField] private DamageZone hitZone;
        [SerializeField] private float deactivationHitTime;

        private PlayerAttack _playerAttack;
        private bool _canHit = true;

        public MeleeDataSO CurrentMelee => _playerAttack.CurrentWeapon.WeaponType == WeaponType.MELEE
            ? _playerAttack.CurrentWeapon.GetCurrentWeaponSO<MeleeDataSO>()
            : null;

        [Inject]
        private void Construct(PlayerAttack playerAttack) => _playerAttack = playerAttack;

        public void Hit()
        {
            if (!_canHit || !CurrentMelee)
                return;

            handsAnim.SetTrigger(_meleeHit);
            hitAnim.SetTrigger(_hitKey);
            StartCoroutine(HitZoneCooldown());
            StartCoroutine(HitCooldown());
        }

        private IEnumerator HitCooldown()
        {
            _canHit = false;
            yield return new WaitForSeconds(CurrentMelee.Cooldown);
            _canHit = true;
        }

        private IEnumerator HitZoneCooldown()
        {
            hitZone.ActivateZone();
            yield return new WaitForSeconds(deactivationHitTime);
            hitZone.DisableZone();
        }
    }
}