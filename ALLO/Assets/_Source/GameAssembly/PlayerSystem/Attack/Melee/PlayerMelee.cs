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
        private static readonly int _meleeHitKey = Animator.StringToHash("MeleeHit");
        private static readonly int _knifeHitKey = Animator.StringToHash("KnifeHit");

        [SerializeField] private Animator handsAnim;
        [SerializeField] private Animator hitAnim;
        [SerializeField] private DamageZone areaHitZone;
        [SerializeField] private DamageZone straightHitZone;
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

            if (CurrentMelee.MeleeType == MeleeType.AREA_ATTACK)
            {
                AreaAttack();
                StartCoroutine(HitZoneCooldown(areaHitZone));
            }
            else
            {
                StraightAttack();
                StartCoroutine(HitZoneCooldown(straightHitZone));
            }


            StartCoroutine(HitCooldown());
        }

        private void AreaAttack()
        {
            handsAnim.SetTrigger(_meleeHitKey);
            hitAnim.SetTrigger(_hitKey);
        }

        private void StraightAttack()
        {
            handsAnim.SetTrigger(_knifeHitKey);
        }

        private IEnumerator HitCooldown()
        {
            _canHit = false;
            yield return new WaitForSeconds(CurrentMelee.Cooldown);
            _canHit = true;
        }

        private IEnumerator HitZoneCooldown(DamageZone damageZone)
        {
            damageZone.ActivateZone();
            yield return new WaitForSeconds(deactivationHitTime);
            damageZone.DisableZone();
        }
    }
}