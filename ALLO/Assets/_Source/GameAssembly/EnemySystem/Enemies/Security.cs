using DamageSystem;
using DamageSystem.Data;
using UnityEngine;

namespace EnemySystem.Enemies
{
	public class Security : AEnemy
	{
		[SerializeField] private DamageZone damageZone;
		[SerializeField] private DamageOwner currentOwner;
		[SerializeField] private int hitDamage;

		private void Start() => damageZone.SetDamage(currentOwner, hitDamage);

		protected override void OnTargetSpotted(Transform target)
		{
		}

		private void Update()
		{
			if (Vision.CurrentTarget)
				SetDestination(Vision.CurrentTarget.position);
		}
	}
}