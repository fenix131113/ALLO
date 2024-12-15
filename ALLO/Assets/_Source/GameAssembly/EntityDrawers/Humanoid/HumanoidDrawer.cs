using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HumanoidDrawer : MonoBehaviour
	{
		[field: SerializeField] public Transform LookTarget { get; private set; }
		[field: SerializeField] public Vector3? LookPosition { get; private set; }
		[field: SerializeField] public Transform CenterPoint { get; private set; }

		protected float GetLookDegrees()
		{
			Vector2 lookDirection = LookTarget ? LookTarget.position - CenterPoint.position : (Vector3)LookPosition - CenterPoint.position;
			
			return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
		}
		
		public void SetLookTarget(Transform target)
		{
			LookTarget = target;
			LookPosition = null;
		}

		public void SetLookTarget(Vector3 position)
		{
			LookTarget = null;
			LookPosition = position;
		}
	}
}