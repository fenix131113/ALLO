using System;
using Core.Services;
using UnityEngine;

namespace EnemySystem
{
	public class EnemyVision : MonoBehaviour
	{
		[SerializeField] private LayerMask targetingLayer;

		public event Action<Transform> OnTargetSpotted; 

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (LayerService.CheckLayersEquality(other.gameObject.layer, targetingLayer))
			{
				OnTargetSpotted?.Invoke(other.transform);
				Debug.Log("Enemy Spotted");
			}
		}
	}
}