using System.Collections.Generic;
using UnityEngine;

namespace EntityDrawers.Humanoid
{
	public class HumanoidHandsDrawer : HumanoidDrawer
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
			if (GetLookDegrees() is < 90 and > -90)
				SwapHandsRight();
			else
				SwapHandsLeft();

			if (GetLookDegrees() is > 15 and < 180)
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
			rightHand.transform.localScale =
				new Vector3(rightHand.transform.localScale.x, 1, rightHand.transform.localScale.z);
			leftHand.transform.localScale =
				new Vector3(leftHand.transform.localScale.x, 1, leftHand.transform.localScale.z);
		}

		private void SwapHandsLeft()
		{
			rightHand.transform.localScale =
				new Vector3(rightHand.transform.localScale.x, -1, rightHand.transform.localScale.z);
			leftHand.transform.localScale =
				new Vector3(leftHand.transform.localScale.x, -1, leftHand.transform.localScale.z);
		}

		private void SetItemsOrder(int order)
		{
			foreach (var item in _handsItems)
				item.sortingOrder = order;
		}
	}
}