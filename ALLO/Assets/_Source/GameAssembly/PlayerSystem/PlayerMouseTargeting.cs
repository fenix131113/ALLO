using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMouseTargeting : MonoBehaviour
	{
		[SerializeField] private Camera targetingCamera;

		private PlayerMutation _playerMutation;
		private PlayerMovement _playerMovement;

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
		private void Construct(PlayerMutation playerMutation, PlayerMovement playerMovement)
		{
			_playerMutation = playerMutation;
			_playerMovement = playerMovement;
		}


		private void FixedUpdate()
		{
			var mousePosition = targetingCamera.ScreenToWorldPoint(Input.mousePosition);

			LookDirection = mousePosition - _playerMutation.CurrentPlayer.LookRotationPivot.position;

			LookDegrees = Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg;

			_playerMutation.CurrentPlayer.LookRotationPivot.rotation = Quaternion.Euler(0, 0, LookDegrees);

			ChangeRotation();
		}

		private void Start() => Cursor.visible = false;

		private void ChangeRotation()
		{
			_playerMutation.CurrentPlayer.BodyDrawer.Rotate(LookDegrees);
			_playerMutation.CurrentPlayer.BodyDrawer.SetMovementDirection(_playerMovement.CurrentMovementVector.magnitude != 0 ? LookDirection : Vector2.zero);
		}
	}
}