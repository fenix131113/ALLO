using System.Collections.Generic;
using EntityDrawers.Humanoid.Data;
using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HandsDrawerBase : DrawerBase
	{
		[SerializeField] private SpriteRenderer rightHand;
		[SerializeField] private SpriteRenderer leftHand;
		
		[Header("Orders")] [SerializeField] private int defaultItemOrder;
		[SerializeField] private int defaultRightHandOrder;
		[SerializeField] private int defaultLeftHandOrder;
		[SerializeField] private int behindItemOrder;
		[SerializeField] private int behindRightHandOrder;
		[SerializeField] private int behindLeftHandOrder;

		private readonly List<SpriteRenderer> _handsItems = new();

		private bool _isHandsRight = true;

		private void Awake()
		{
			for (var i = 0; i < rightHand.transform.childCount; i++)
				_handsItems.Add(rightHand.transform.GetChild(i).GetComponent<SpriteRenderer>());
		}

		private void FixedUpdate()
		{
			CheckHandsDraw();
		}

		private void CheckHandsDraw()
		{
			if(!LookTarget && LookPosition == null)
				return;

			var lookDegrees = GetLookDegrees();
			
			if (lookDegrees is < 90 and > -90)
				SwapHandsRight();
			else
				SwapHandsLeft();

			if (lookDegrees is > 15 and < 180)
				SwapHandsBehind();
			else
				SwapHandsForward();
		}

		private void SwapHandsBehind()
		{
			rightHand.sortingOrder = behindRightHandOrder;
			leftHand.sortingOrder = behindLeftHandOrder;
			SetItemsOrder(behindItemOrder);
		}

		private void SwapHandsForward()
		{
			rightHand.sortingOrder = defaultRightHandOrder;
			leftHand.sortingOrder = defaultLeftHandOrder;
			SetItemsOrder(defaultItemOrder);
		}

		private void SwapHandsRight()
		{
			if (_isHandsRight)
				return;

			_isHandsRight = true;
			
			var newScaleVector = new Vector3(rightHand.transform.localScale.x, 1, rightHand.transform.localScale.z);

			CenterPoint.localScale = newScaleVector;
			
			// rightHand.transform.localScale = newScaleVector;
			// leftHand.transform.localScale = newScaleVector;
			//
			// var tempPosRight = rightHand.transform.localPosition;
			// rightHand.transform.localPosition = new Vector3(tempPosRight.x, tempPosRight.y / -1, tempPosRight.z);
			//
			// var tempPosLeft = leftHand.transform.localPosition;
			// leftHand.transform.localPosition = new Vector3(tempPosLeft.x, tempPosLeft.y / -1, tempPosLeft.z);
		}

		private void SwapHandsLeft()
		{
			if (!_isHandsRight)
				return;

			_isHandsRight = false;
			
			var newScaleVector = new Vector3(rightHand.transform.localScale.x, -1, rightHand.transform.localScale.z);
			
			CenterPoint.localScale = newScaleVector;

			// rightHand.transform.localScale = newScaleVector;
			// leftHand.transform.localScale = newScaleVector;
			//
			// var tempPosRight = rightHand.transform.localPosition;
			// rightHand.transform.localPosition = new Vector3(tempPosRight.x, tempPosRight.y * -1, tempPosRight.z);
			//
			// var tempPosLeft = leftHand.transform.localPosition;
			// leftHand.transform.localPosition = new Vector3(tempPosLeft.x, tempPosLeft.y * -1, tempPosLeft.z);
		}

		private void SetItemsOrder(int order)
		{
			foreach (var item in _handsItems)
				item.sortingOrder = order;
		}

		public override void GlowEffect(float time)
		{
			throw new System.NotImplementedException();
		}

		protected override void Rotate(HumanoidRotationsEnum rotation)
		{
			throw new System.NotImplementedException();
		}

		public override void Rotate(float degrees)
		{
			throw new System.NotImplementedException();
		}

		public override void SetCurrentMovement(Vector2 movementVector, bool run)
		{
			throw new System.NotImplementedException();
		}

		public override void SetRunState(bool state)
		{
			throw new System.NotImplementedException();
		}

		public override void SetMovementDirection(Vector2 movementVector)
		{
			throw new System.NotImplementedException();
		}
	}
}