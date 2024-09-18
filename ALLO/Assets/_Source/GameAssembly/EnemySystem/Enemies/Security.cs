using UnityEngine;

namespace EnemySystem.Enemies
{
	public class Security : AEnemy
	{
		private Transform _target;
		protected override void OnTargetSpotted(Transform target)
		{
			_target = target;
		}

		private void Update()
		{
			if(_target)
				SetDestination(_target.position);
		}
	}
}