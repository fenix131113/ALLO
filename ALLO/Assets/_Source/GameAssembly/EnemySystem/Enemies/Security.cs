using System.Collections;
using DamageSystem;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using PlayerSystem;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Items.Collectable;
using UnityEngine;
using Utils;
using Zenject;

namespace EnemySystem.Enemies
{
    public class Security : AEnemy
    {
        private static readonly int Hit = Animator.StringToHash("Hit");

        [SerializeField] private CollectableAmmoBox ammoBoxPrefab;
        [SerializeField] private CollectableFirstAid firstAidPrefab;
        [SerializeField] private BodyDrawerBase bodyDrawerBase;
        [SerializeField] private HandsDrawerBase handsDrawerBase;
        [SerializeField] private ExtraLifeModule extraLifeModule;
        [SerializeField] private DamageZone damageZone;
        [SerializeField] private ParticleSystem fleshParticles;
        [SerializeField] private Animator hitEffectAnimator;
        [SerializeField] private Animator hitAnimator;
        [SerializeField] private int hitDamage;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float hitGlowTime;

        private PlayerAmmoContainer _playerAmmoContainer;
        private PlayerMutation _playerMutation;
        private int _extraLifeUsed;
        private float _attackCooldownTimer;
        private bool _canAttack = true;
        private bool _isAlwaysSeePlayer;
        private Vector3? _walkToPosition;

        [Inject]
        public void Construct(PlayerAmmoContainer playerAmmoContainer, PlayerMutation playerMutation)
        {
            _playerAmmoContainer = playerAmmoContainer;
            _playerMutation = playerMutation;
        }

        private void Start()
        {
            damageZone.SetDamage(Owner, hitDamage);
            AiPath.maxSpeed = Random.Range(AiPath.maxSpeed - 0.5f, AiPath.maxSpeed + 0.5f);
        }

        private void Update()
        {
            if (_isAlwaysSeePlayer)
                SetDestination(Vision.CurrentTarget.position);

            if (AiPath.reachedEndOfPath && Vision.CanSeeTarget)
                Attack();

            bodyDrawerBase.SetCurrentMovement(AiPath.velocity, true);

            if (Vision.CanSeeTarget || _walkToPosition == null || _isAlwaysSeePlayer)
                return;

            Vector2 lookDirection = (Vector3)_walkToPosition - transform.position;

            var lookDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            bodyDrawerBase.Rotate(lookDegrees);
            handsDrawerBase.SetLookTarget((Vector3)_walkToPosition);
        }

        public void SetIsAlwaysSeePlayer(bool state) => _isAlwaysSeePlayer = state;


        protected override void OnTargetSpotted(Transform target)
        {
            handsDrawerBase.SetLookTarget(target);
            LookAtTarget();
        }

        protected override void OnTargetLost(Transform target)
        {
            if (_isAlwaysSeePlayer)
                return;

            _walkToPosition = target.position;
            SetDestination(target.position);
            handsDrawerBase.SetLookTarget((Vector3)_walkToPosition);
        }

        protected override void OnSeeTarget(Transform target)
        {
            if (_isAlwaysSeePlayer)
                return;

            SetDestination(Vision.CurrentTarget.position);
            LookAtTarget();
        }

        protected override void Die()
        {
            //TODO: Delete hardcode
            if (Random.Range(0f, 1f) <= 0.07f)
                Instantiate(firstAidPrefab, transform.position, Quaternion.identity);
            else if (Random.Range(0f, 1f) <= 0.15f)
                Instantiate(ammoBoxPrefab, transform.position, Quaternion.identity).Init(_playerAmmoContainer);

            _playerMutation.IncreaseKillsCount();
            fleshParticles.DestroyByTime(fleshParticles.main.duration);
            fleshParticles.Play();
            fleshParticles.transform.parent = null;
            fleshParticles.gameObject.transform.localScale = Vector3.one;
            Destroy(gameObject);
        }

        private void LookAtTarget()
        {
            Vector2 lookDirection = Vision.CurrentTarget.position - handsDrawerBase.CenterPoint.position;

            var lookDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            handsDrawerBase.CenterPoint.rotation = Quaternion.Euler(0, 0, lookDegrees);

            bodyDrawerBase.SetCurrentMovement(AiPath.velocity, true);
            bodyDrawerBase.Rotate(lookDegrees);
        }

        public override void TakeDamage(int damage)
        {
            damage = Mathf.Clamp(damage, 0, Health);
            Health -= damage;
            bodyDrawerBase.GlowEffect(hitGlowTime);

            switch (Health)
            {
                case 0 when (extraLifeModule.ExtraLifeGroups.Count == _extraLifeUsed ||
                             !extraLifeModule.CanGetExtraLife(_extraLifeUsed)):
                    Die();
                    break;
                case 0:
                    _extraLifeUsed += 1;
                    break;
            }
        }

        private void Attack()
        {
            if (!_canAttack)
                return;

            AttackEffect();
            damageZone.ActivateZone();

            StartCoroutine(AttackCooldown());
        }

        private void AttackEffect()
        {
            hitEffectAnimator.SetTrigger(Hit);
            hitAnimator.SetTrigger(Hit);
        }

        private IEnumerator AttackCooldown()
        {
            _canAttack = false;

            yield return new WaitForSeconds(attackCooldown);

            _canAttack = true;
        }
    }
}