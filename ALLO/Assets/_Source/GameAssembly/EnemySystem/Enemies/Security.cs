using System.Collections;
using DamageSystem;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using PlayerSystem.Items;
using UnityEngine;
using Utils;

namespace EnemySystem.Enemies
{
	public class Security : AEnemy
	{
		private static readonly int Hit = Animator.StringToHash("Hit");

		[SerializeField] private CollectableAmmoBox ammoBoxPrefab;
		[SerializeField] private CollectableFirstAid firstAidPrefab;
		[SerializeField] private HumanoidBodyDrawer bodyDrawer;
		[SerializeField] private HumanoidHandsDrawer handsDrawer;
		[SerializeField] private ExtraLifeModule extraLifeModule;
		[SerializeField] private DamageZone damageZone;
		[SerializeField] private ParticleSystem fleshParticles;
		[SerializeField] private Animator hitEffectAnimator;
		[SerializeField] private Animator hitAnimator;
		[SerializeField] private int hitDamage;
		[SerializeField] private float attackCooldown;
		[SerializeField] private float hitGlowTime;

		private int _damageCounter;
		private float _attackCooldownTimer;
		private bool _canAttack = true;

		private void Start()
		{
			damageZone.SetDamage(Owner, hitDamage);
			AiPath.maxSpeed = Random.Range(AiPath.maxSpeed - 0.5f, AiPath.maxSpeed + 0.5f);
		}

		protected override void OnTargetSpotted(Transform target)
		{
			handsDrawer.SetLookTarget(target);
		}

		protected override void Die()
		{
			if(Random.Range(0f, 1f) <= 0.07f)
				Instantiate(firstAidPrefab, transform.position, Quaternion.identity);
			else if(Random.Range(0f, 1f) <= 0.15f)
				Instantiate(ammoBoxPrefab, transform.position, Quaternion.identity);
			
			fleshParticles.DestroyByTime(fleshParticles.main.duration);
			fleshParticles.Play();
			fleshParticles.transform.parent = null;
			fleshParticles.gameObject.transform.localScale = Vector3.one;
			Destroy(gameObject);
		}

		private void LookAtTarget()
		{
			if (!Vision.CurrentTarget)
			{
				bodyDrawer.SetCurrentMovement(Vector2.zero, true);
				return;
			}

			Vector2 lookDirection = Vision.CurrentTarget.position - handsDrawer.CenterPoint.position;

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

		private void FixedUpdate()
		{
			LookAtTarget();

			if (!Vision.CurrentTarget)
				return;

			SetDestination(Vision.CurrentTarget.position);
		}

		private void Update()
		{
			if (AiPath.reachedEndOfPath)
				Attack();
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