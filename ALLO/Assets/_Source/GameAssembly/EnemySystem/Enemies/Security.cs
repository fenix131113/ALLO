using System.Collections;
using DamageSystem;
using UnityEngine;

namespace EnemySystem.Enemies
{
	public class Security : AEnemy
	{
		[SerializeField] private DamageZone damageZone;
		[SerializeField] private int hitDamage;
		[SerializeField] private float attackCooldown;
		
		private float _attackCooldownTimer;
		private bool _canAttack = true;

		private void Start() => damageZone.SetDamage(Owner, hitDamage);

		protected override void OnTargetSpotted(Transform target)
		{
		}

		public override void TakeDamage(int damage)
		{
			Health -= damage;
			Health = Mathf.Clamp(Health, 0, MaxHealth);
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