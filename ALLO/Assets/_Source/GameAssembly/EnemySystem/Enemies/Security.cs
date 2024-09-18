using System.Collections;
using DamageSystem;
using DamageSystem.Data;
using UnityEngine;

namespace EnemySystem.Enemies
{
	public class Security : AEnemy
	{
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
		}

		protected override void Die()
		{
			Destroy(gameObject);
		}

		public override void TakeDamage(int damage)
		{
			damage = Mathf.Clamp(damage, 0, Health);
			Health -= damage;
			_damageCounter += damage;
			
			if(!extraLifeModule.CanGetExtraLife(_damageCounter))
				Die();
		}

		private void Update()
		{
			if (!Vision.CurrentTarget)
				return;

			SetDestination(Vision.CurrentTarget.position);

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