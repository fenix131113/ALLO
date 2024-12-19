using System.Collections;
using DamageSystem;
using DG.Tweening;
using EntityDrawers.Humanoid;
using PlayerSystem.Attack.Throwable;
using UnityEngine;

namespace EntityDrawers
{
    //TODO: Separate mutant logic
    public class MutantDrawer : DrawerBase
    {
        private static readonly int _blendTreeX = Animator.StringToHash("X");
        private static readonly int _blendTreeY = Animator.StringToHash("Y");
        private static readonly int _hit = Animator.StringToHash("Hit");
        
        [SerializeField] private SpriteRenderer hitRenderer;
        [SerializeField] private Spit spitPrefab;
        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private Animator bodyAnimator;
        [SerializeField] private Color glowColor;
        [SerializeField] private Animator hitAnimator;
        [SerializeField] private DamageZone damageZone;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float spitCooldown;

        private bool _runState;
        private bool _canAttack = true;
        private bool _canSpit = true;
        private bool _spitUnlocked;
        private Vector2 _moveDirection;

        public void Attack()
        {
            if(!_canAttack)
                return;
            
            damageZone.ActivateZone();
            hitAnimator.SetTrigger(_hit);
            _canAttack = false;
            StartCoroutine(AttackCooldown());
        }

        public void Spit()
        {
            if(!_spitUnlocked || !_canSpit)
                return;
            
            //Spit logic
            Instantiate(spitPrefab, CenterPoint.position, CenterPoint.rotation);
            _canSpit = false;
            StartCoroutine(SpitCooldown());
        }

        public void UnlockSpit() => _spitUnlocked = true;

        public void SetAttackState(bool state) => _canAttack = state;

        public override void GlowEffect(float time)
        {
            var glowSeq = DOTween.Sequence();
            glowSeq.Append(bodyRenderer.DOColor(glowColor, time));

            glowSeq.onComplete += () =>
            {
                var unGlowSeq = DOTween.Sequence();
                unGlowSeq.Append(bodyRenderer.DOColor(Color.white, time));
            };
        }

        public void ResetHitEffect()
        {
            damageZone.DisableZone();
            hitRenderer.enabled = false;
        }

        private void ChangeRenderers()
        {
            bodyAnimator.SetFloat(_blendTreeX, _moveDirection.normalized.x);
            bodyAnimator.SetFloat(_blendTreeY, _moveDirection.normalized.y);

            bodyAnimator.enabled = _moveDirection.magnitude != 0;
        }

        public override void SetMovementDirection(Vector2 movementVector) => _moveDirection = movementVector;

        public override void SetCurrentMovement(Vector2 movementVector, bool run)
        {
            SetMovementDirection(movementVector);
            SetRunState(run);
            ChangeRenderers();
        }

        public override void SetRunState(bool state)
        {
            _runState = state;
            CheckAnimatorSpeed();
        }

        private void CheckAnimatorSpeed() => bodyAnimator.speed = _runState ? 1f : 0.5f;

        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        
        private IEnumerator SpitCooldown()
        {
            yield return new WaitForSeconds(spitCooldown);
            _canSpit = true;
        }
    }
}