using System;
using EntityDrawers.Humanoid.Data;
using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HumanoidBodyDrawer : HumanoidDrawer
	{
		private static readonly int LegsBlendTreeX = Animator.StringToHash("X");
		private static readonly int LegsBlendTreeY = Animator.StringToHash("Y");

		[SerializeField] private SpriteRenderer headRenderer;
		[SerializeField] private SpriteRenderer bodyRenderer;
		[SerializeField] private SpriteRenderer legsRenderer;
		[SerializeField] private Animator legsAnimator;

		/// <summary>
		/// 1 - Down<br/>
		/// 2 - Down_Right<br/>
		/// 3 - Right<br/>
		/// 4 - Right_Up<br/>
		/// 5 - Up<br/>
		/// 6 - Up_Left<br/>
		/// 7 - Left<br/>
		/// 8 - Left_Down
		/// </summary>
		[Tooltip("1 - Down\n2 - Down_Right\n3 - Right\n4 - Right_Up\n5 - Up\n6 - Up_Left\n7 - Left\n8 - Left_Down")]
		[SerializeField]
		private HumanoidBodyRotationGroup[] rotations = new HumanoidBodyRotationGroup[8];

		private bool _runState;
		private Vector2 _moveDirection;
		private float _currentRotateDegrees;

		public void Rotate(float degrees)
		{
			// Drawer
			HumanoidRotationsEnum newRotation = HumanoidRotationsEnum.DOWN;
			
			_currentRotateDegrees = degrees;

			switch (degrees)
			{
				//Down
				case (> -112.5f and < -67.5f):
					newRotation = HumanoidRotationsEnum.DOWN;
					break;
				//Down_Right
				case (> -67.5f and < -22.5f):
					newRotation = HumanoidRotationsEnum.DOWN_RIGHT;
					break;
				//Right
				case (> -22.5f and < 22.5f):
					newRotation = HumanoidRotationsEnum.RIGHT;
					break;
				//Right_Up
				case (> 22.5f and < 67.5f):
					newRotation = HumanoidRotationsEnum.RIGHT_UP;
					break;
				//Up
				case (> 67.5f and < 112.5f):
					newRotation = HumanoidRotationsEnum.UP;
					break;
				//Up_Left
				case (> 112.5f and < 157.5f):
					newRotation = HumanoidRotationsEnum.UP_LEFT;
					break;
				//Left
				case (> 157.5f or < -157.5f):
					newRotation = HumanoidRotationsEnum.LEFT;
					break;
				//Left_Down
				case (> -157.5f and < -112.5f):
					newRotation = HumanoidRotationsEnum.LEFT_DOWN;
					break;
			}

			Rotate(newRotation);
		}

		private void Rotate(HumanoidRotationsEnum rotation)
		{
			switch (rotation)
			{
				case HumanoidRotationsEnum.DOWN:
					ChangeRenderers(rotations[0]);
					break;
				case HumanoidRotationsEnum.DOWN_RIGHT:
					ChangeRenderers(rotations[1]);
					break;
				case HumanoidRotationsEnum.RIGHT:
					ChangeRenderers(rotations[2]);
					break;
				case HumanoidRotationsEnum.RIGHT_UP:
					ChangeRenderers(rotations[3]);
					break;
				case HumanoidRotationsEnum.UP:
					ChangeRenderers(rotations[4]);
					break;
				case HumanoidRotationsEnum.UP_LEFT:
					ChangeRenderers(rotations[5]);
					break;
				case HumanoidRotationsEnum.LEFT:
					ChangeRenderers(rotations[6]);
					break;
				case HumanoidRotationsEnum.LEFT_DOWN:
					ChangeRenderers(rotations[7]);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
			}
		}

		private void ChangeRenderers(HumanoidBodyRotationGroup rotationGroup)
		{
			headRenderer.sprite = rotationGroup.Head;
			bodyRenderer.sprite = rotationGroup.Body;
			legsAnimator.SetFloat(LegsBlendTreeX, rotationGroup.LegsBlendTreeX);
			legsAnimator.SetFloat(LegsBlendTreeY, rotationGroup.LegsBlendTreeY);

			if (_moveDirection.magnitude == 0)
			{
				legsAnimator.enabled = false;
				legsRenderer.sprite = rotationGroup.Legs;
			}
			else
				legsAnimator.enabled = true;
		}

		private void CheckAnimatorSpeed()
		{
			legsAnimator.speed = _runState ? 1f : 0.5f;
		}

		public void SetCurrentMovement(Vector2 movementVector, bool run)
		{
			_runState = run;
			_moveDirection = movementVector;
			Rotate(_currentRotateDegrees);
			CheckAnimatorSpeed();
		}
	}
}