using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMouseTargeting : MonoBehaviour
	{
		[SerializeField] private Camera targetingCamera;
		
		private PlayerMutation _playerMutation;

		[Inject]
		private void Construct(PlayerMutation playerMutation)
		{
			_playerMutation = playerMutation;
		}
		
		private void FixedUpdate()
		{
			var mousePosition = targetingCamera.ScreenToWorldPoint(Input.mousePosition);
			
			Vector2 lookDirection = mousePosition - _playerMutation.CurrentPlayer.transform.position;
			
			_playerMutation.CurrentPlayer.LookRotationPivot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90);
		}
	}
}