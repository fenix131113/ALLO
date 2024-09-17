using System;
using EntityDrawers.Humanoid.Data;
using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HumanoidBodyDrawer : HumanoidDrawer
	{
		[SerializeField] private SpriteRenderer headRenderer;
		[SerializeField] private SpriteRenderer bodyRenderer;
		[SerializeField] private SpriteRenderer legsRenderer;
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
		[SerializeField] private HumanoidBodyRotationGroup[] rotations = new HumanoidBodyRotationGroup[8];

		public void Rotate(HumanoidRotationsEnum rotation)
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
			legsRenderer.sprite = rotationGroup.Legs;
		}
	}
}