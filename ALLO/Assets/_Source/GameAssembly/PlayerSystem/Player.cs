using EntityDrawers.Humanoid;
using UnityEngine;

namespace PlayerSystem
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Player : MonoBehaviour
	{
		[field: SerializeField] public Rigidbody2D Rb { get; private set; }
		[field: SerializeField] public Transform LookRotationPivot { get; private set; }
		[field: SerializeField] public HumanoidBodyDrawer BodyDrawer { get; private set; }

		public void OnMutated()
		{
		}
	}
}