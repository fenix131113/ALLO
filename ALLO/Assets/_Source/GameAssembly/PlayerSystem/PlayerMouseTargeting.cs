using EntityDrawers.Humanoid.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMouseTargeting : MonoBehaviour
	{
		[SerializeField] private Camera targetingCamera;

		private PlayerMutation _playerMutation;

		public Vector2 LookDirection { get; private set; }

		/// <summary>
		/// 0 = Right<br/>
		/// 90 = Up<br/>
		/// 180 = Left<br/> 
		/// -90 = Down<br/>
		/// -180 - Left
		/// </summary>
		public float LookDegrees { get; private set; }

		[Inject]
		private void Construct(PlayerMutation playerMutation)
		{
			_playerMutation = playerMutation;
		}

		private void FixedUpdate()
		{
			var mousePosition = targetingCamera.ScreenToWorldPoint(Input.mousePosition);

			LookDirection = mousePosition - _playerMutation.CurrentPlayer.LookRotationPivot.position;

			LookDegrees = Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg;

			_playerMutation.CurrentPlayer.LookRotationPivot.rotation = Quaternion.Euler(0, 0, LookDegrees);

			ChangeRotation();
		}

		private void ChangeRotation() => _playerMutation.CurrentPlayer.BodyDrawer.Rotate(LookDegrees);
	}
}