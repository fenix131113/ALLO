using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HumanoidDrawer : MonoBehaviour
	{
		[SerializeField] protected Transform lookTarget;
		[SerializeField] protected Transform centerPoint;
		
		protected float GetLookDegrees()
		
		{
			Vector2 lookDirection = lookTarget.position - centerPoint.position;
			
			return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
		}
	}
}