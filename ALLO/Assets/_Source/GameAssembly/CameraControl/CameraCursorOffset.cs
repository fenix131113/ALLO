using Cinemachine;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace CameraControl
{
	public class CameraCursorOffset : MonoBehaviour
	{
		[SerializeField] private CinemachineTargetGroup targetGroup;
		[SerializeField] private Transform cursorTransform;
		[SerializeField] private Camera cursorCamera;

		private PlayerMutation _playerMutation;

		[Inject]
		private void Construct(PlayerMutation mutation)
		{
			_playerMutation = mutation;
		}

		private void Awake()
		{
			Bind();
		}

		private void Bind()
		{
			_playerMutation.OnMutated += CheckPlayerTarget;
		}

		private void Expose()
		{
			_playerMutation.OnMutated -= CheckPlayerTarget;
		}

		private void CheckPlayerTarget()
		{
			targetGroup.m_Targets[0].target = _playerMutation.CurrentPlayer.transform;
		}
		
		private void FixedUpdate()
		{
			Vector3 point = cursorCamera.ScreenToWorldPoint(Input.mousePosition);
			cursorTransform.position = new Vector3(point.x, point.y, 0);
		}
		
		private void OnDestroy() => Expose();
		private void OnApplicationQuit() => Expose();
	}
}