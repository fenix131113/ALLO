using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HumanoidDrawer : MonoBehaviour
	{
		[field: SerializeField] public Transform LookTarget { get; private set; }
		[field: SerializeField] public Transform CenterPoint { get; private set; }

		protected float GetLookDegrees()
		{
			Vector2 lookDirection = LookTarget.position - CenterPoint.position;
			
			return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
		}
		
		public void SetLookTarget(Transform target) => LookTarget = target;
	}
}