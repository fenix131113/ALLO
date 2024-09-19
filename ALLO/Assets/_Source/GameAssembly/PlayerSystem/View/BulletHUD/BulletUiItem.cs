using UnityEngine;

namespace PlayerSystem.View.BulletHUD
{
	public class BulletUiItem : MonoBehaviour
	{
		[field: SerializeField] public RectTransform Rect { get; private set; }

		[SerializeField] private float rotateSpeed;

		private bool _canRotate;

		public void StartRotate() => _canRotate = true;

		private void Update()
		{
			if (_canRotate)
				transform.Rotate(-Vector3.forward, Time.deltaTime * rotateSpeed);
		}
	}
}