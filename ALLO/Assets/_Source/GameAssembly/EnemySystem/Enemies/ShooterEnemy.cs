using System.Collections;
using System.Linq;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using PlayerSystem;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Shooting.Data;
using UnityEngine;
using Utils;
using Zenject;
using Random = UnityEngine.Random;

namespace EnemySystem.Enemies
{
    public class ShooterEnemy : AEnemy
    {
        [SerializeField] private FirearmsDataSO weaponData;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private BodyDrawerBase bodyDrawerBase;
        [SerializeField] private HandsDrawerBase handsDrawerBase;
        [SerializeField] private ExtraLifeModule extraLifeModule;
        [SerializeField] private ParticleSystem fleshParticles;
        [SerializeField] private EnemyVision escapeZoneVision;
        [SerializeField] private CircleCollider2D escapeZoneVisionCollider;
        [SerializeField] private float escapeOffset;
        [SerializeField] private float shootSpread;
        [SerializeField] private int hitDamage;
        [SerializeField] private float shootCooldown;
        [SerializeField] private float hitGlowTime;
        [SerializeField] private int ammoInClip;
        [SerializeField] private float reloadTime;
        
        private int _extraLifeUsed;
        private float _attackCooldownTimer;
        private bool _canShoot = true;
        private bool _isReloading;
        private Vector3? _walkToPosition;
        private int _ammoLeft;
        private DiContainer _diContainer;
        
        [Inject]
        public void Construct(DiContainer diContainer) => _diContainer = diContainer;

        private void Start()
        {
            AiPath.maxSpeed = Random.Range(AiPath.maxSpeed, AiPath.maxSpeed + 0.5f);
            BindAdditional();
            _ammoLeft = ammoInClip;
        }

        private void Update()
        {
            if (Vision.CanSeeTarget || escapeZoneVision.CanSeeTarget)
            {
                _walkToPosition = Vision.CurrentTarget.position;
                Shoot();
            }
            else if (_walkToPosition != null)
            {
                SetDestination((Vector3)_walkToPosition);
                _walkToPosition = null;
            }

            bodyDrawerBase.SetCurrentMovement(AiPath.velocity, true);

            if ((Vision.CanSeeTarget && escapeZoneVision.CanSeeTarget) || _walkToPosition == null)
                return;

            Vector2 lookDirection = (Vector3)_walkToPosition - transform.position;

            var lookDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            bodyDrawerBase.Rotate(lookDegrees);
            handsDrawerBase.SetLookTarget((Vector3)_walkToPosition);
        }

        protected override void OnTargetSpotted(Transform target)
        {
            FollowDistanceTarget(target);
            escapeZoneVision.NativeSetTarget(target, false);
        }

        protected override void OnTargetLost(Transform target)
        {
            if (_walkToPosition != null)
                handsDrawerBase.SetLookTarget((Vector3)_walkToPosition);
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
            handsDrawerBase.SetLookTarget(target);
            LookAtTarget();
        }

        protected override void Die()
        {
            GenerateDropObject();
            ExposeAdditional();
            fleshParticles.DestroyByTime(fleshParticles.main.duration);
            fleshParticles.Play();
            fleshParticles.transform.parent = null;
            fleshParticles.gameObject.transform.localScale = Vector3.one;
            Destroy(gameObject);
        }
        
        private void GenerateDropObject()
        {
            var weightSum = DropGroups.Sum(item => item.Weight);
            var sortedGroups = DropGroups.OrderByDescending(item => item.Weight).ToList();
            
            foreach (var group in sortedGroups)
            {
                if (Random.Range(0, weightSum + 1) <= group.Weight)
                    continue;

                if (group.DropObject)
                    _diContainer.InjectGameObject(
                        Instantiate(group.DropObject, transform.position, Quaternion.identity));
                return;
            }
            
            if (sortedGroups[^1].DropObject)
                _diContainer.InjectGameObject(
                    Instantiate(sortedGroups[^1].DropObject, transform.position, Quaternion.identity));
        }

        private void LookAtTarget()
        {
            Vector2 lookDirection = Vision.CurrentTarget.position - transform.position;

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
                case 0 when extraLifeModule.ExtraLifeGroups.Count == _extraLifeUsed ||
                            !extraLifeModule.CanGetExtraLife(_extraLifeUsed):
                    Die();
                    break;
                case 0:
                    _extraLifeUsed += 1;
                    break;
            }
        }

        private void Shoot()
        {
            if (!_canShoot || _isReloading)
                return;

            //Spawn bullet with spread applied
            var finalRotation = handsDrawerBase.CenterPoint.rotation.eulerAngles + Vector3.forward *
                Random.Range(-shootSpread, shootSpread);

            var bullet = Instantiate(weaponData.BulletPrefab, shootPoint.position, Quaternion.Euler(finalRotation));
            bullet.SetDamageOwner(Owner);
            bullet.SetDamage(hitDamage);
            _ammoLeft--;
            StartCoroutine(ShootCooldown());

            if (_ammoLeft == 0 && ammoInClip > 0)
                StartCoroutine(ReloadCooldown());
        }

        private void BindAdditional() => escapeZoneVision.OnTargetSpotted += OnTargetInEscapeZone;

        private void ExposeAdditional() => escapeZoneVision.OnTargetSpotted -= OnTargetInEscapeZone;

        private IEnumerator ShootCooldown()
        {
            _canShoot = false;

            yield return new WaitForSeconds(shootCooldown);

            _canShoot = true;
        }

        private IEnumerator ReloadCooldown()
        {
            _isReloading = true;
            
            yield return new WaitForSeconds(reloadTime);

            _ammoLeft = ammoInClip;
            _isReloading = false;
        }
    }
}