using System.Collections;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Shooting.Data;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace EnemySystem.Enemies
{
    public class ShooterEnemy : AEnemy
    {
        [SerializeField] private FirearmsDataSO weaponData;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private HumanoidBodyDrawer bodyDrawer;
        [SerializeField] private HumanoidHandsDrawer handsDrawer;
        [SerializeField] private ExtraLifeModule extraLifeModule;
        [SerializeField] private ParticleSystem fleshParticles;
        [SerializeField] private EnemyVision escapeZoneVision;
        [SerializeField] private CircleCollider2D escapeZoneVisionCollider;
        [SerializeField] private float escapeOffset;
        [SerializeField] private float shootSpread;
        [SerializeField] private int hitDamage;
        [SerializeField] private float shootCooldown;
        [SerializeField] private float hitGlowTime;

        private PlayerAmmoContainer _playerAmmoContainer;
        private int _damageCounter;
        private float _attackCooldownTimer;
        private bool _canShoot = true;
        private Vector3? _walkToPosition;

        private void Start()
        {
            AiPath.maxSpeed = Random.Range(AiPath.maxSpeed, AiPath.maxSpeed + 0.5f);
            BindAdditional();
        }

        private void Update()
        {
            if (Vision.CanSeeTarget || escapeZoneVision.CanSeeTarget)
                Shoot();

            bodyDrawer.SetCurrentMovement(AiPath.velocity, true);

            if ((Vision.CanSeeTarget && escapeZoneVision.CanSeeTarget) || _walkToPosition == null)
                return;

            Vector2 lookDirection = (Vector3)_walkToPosition - transform.position;

            var lookDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            bodyDrawer.Rotate(lookDegrees);
            handsDrawer.SetLookTarget((Vector3)_walkToPosition);
        }

        protected override void OnTargetSpotted(Transform target)
        {
            FollowDistanceTarget(target);
            escapeZoneVision.NativeSetTarget(target, false);
        }

        protected override void OnTargetLost(Transform target)
        {
            _walkToPosition = target.position;
            SetDestination(target.position);
            handsDrawer.SetLookTarget((Vector3)_walkToPosition);
        }

        protected override void OnSeeTarget(Transform target)
        {
            FollowDistanceTarget(target);
        }

        private void OnTargetInEscapeZone(Transform target)
        {
            Vision.NativeSetTarget(target);
            FollowDistanceTarget(target);
        }

        private void FollowDistanceTarget(Transform target)
        {
            if (!escapeZoneVision.CanSeeTarget)
                return;

            var direction = transform.position - target.position;
            direction.Normalize();
            direction *= escapeZoneVisionCollider.radius + escapeOffset;
            SetDestination(target.position + direction);
            handsDrawer.SetLookTarget(target);
            LookAtTarget();
        }

        protected override void Die()
        {
            ExposeAdditional();
            fleshParticles.DestroyByTime(fleshParticles.main.duration);
            fleshParticles.Play();
            fleshParticles.transform.parent = null;
            fleshParticles.gameObject.transform.localScale = Vector3.one;
            Destroy(gameObject);
        }

        private void LookAtTarget()
        {
            Vector2 lookDirection = Vision.CurrentTarget.position - transform.position;

            var lookDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            handsDrawer.CenterPoint.rotation = Quaternion.Euler(0, 0, lookDegrees);

            bodyDrawer.SetCurrentMovement(AiPath.velocity, true);
            bodyDrawer.Rotate(lookDegrees);
        }

        public override void TakeDamage(int damage)
        {
            damage = Mathf.Clamp(damage, 0, Health);
            Health -= damage;
            _damageCounter += damage;
            bodyDrawer.GlowEffect(hitGlowTime);

            if (!extraLifeModule.CanGetExtraLife(_damageCounter))
                Die();
        }

        private void Shoot()
        {
            if (!_canShoot)
                return;

            //Spawn bullet with spread applied
            var finalRotation = handsDrawer.CenterPoint.rotation.eulerAngles + Vector3.forward *
                Random.Range(-shootSpread, shootSpread);

            var bullet = Instantiate(weaponData.BulletPrefab, shootPoint.position, Quaternion.Euler(finalRotation));
            bullet.SetDamageOwner(Owner);
            bullet.SetDamage(weaponData.Damage);

            StartCoroutine(ShootCooldown());
        }

        private void BindAdditional() => escapeZoneVision.OnTargetSpotted += OnTargetInEscapeZone;

        private void ExposeAdditional() => escapeZoneVision.OnTargetSpotted -= OnTargetInEscapeZone;

        private IEnumerator ShootCooldown()
        {
            _canShoot = false;

            yield return new WaitForSeconds(shootCooldown);

            _canShoot = true;
        }
    }
}