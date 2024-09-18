using System.Collections;
using DamageSystem;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using UnityEngine;

namespace EnemySystem.Enemies
{
	public class Security : AEnemy
	{
		[SerializeField] private HumanoidBodyDrawer bodyDrawer;
		[SerializeField] private HumanoidHandsDrawer handsDrawer;
		[SerializeField] private ExtraLifeModule extraLifeModule;
		[SerializeField] private DamageZone damageZone;
		[SerializeField] private int hitDamage;
		[SerializeField] private float attackCooldown;
		
		private int _damageCounter;
		private float _attackCooldownTimer;
		private bool _canAttack = true;

		private void Start() => damageZone.SetDamage(Owner, hitDamage);
		
		protected override void OnTargetSpotted(Transform target)
		{
			handsDrawer.SetLookTarget(target);
		}

		protected override void Die()
		{
			Destroy(gameObject);
		}

		private void LookAtTarget()
		{
			if (!handsDrawer.LookTarget)
			{
				bodyDrawer.SetCurrentMovement(Vector2.zero, true);
				return;
			}

			Vector2 lookDirection = handsDrawer.LookTarget.position - handsDrawer.CenterPoint.position;

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
			
			if(!extraLifeModule.CanGetExtraLife(_damageCounter))
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

			damageZone.ActivateZone();

			StartCoroutine(AttackCooldown());
		}

		private IEnumerator AttackCooldown()
		{
			_canAttack = false;

			yield return new WaitForSeconds(attackCooldown);

			_canAttack = true;
		}
	}
}