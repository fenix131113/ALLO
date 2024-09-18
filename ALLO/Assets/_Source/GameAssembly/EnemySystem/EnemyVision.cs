using System;
using Core.Services;
using UnityEngine;

namespace EnemySystem
{
	public class EnemyVision : MonoBehaviour
	{
		[SerializeField] private LayerMask targetingLayer;
		
		public Transform CurrentTarget { get; private set; }

		public event Action<Transform> OnTargetSpotted; 

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetingLayer) || CurrentTarget) return;
			
			OnTargetSpotted?.Invoke(other.transform);
			CurrentTarget = other.transform;
		}
	}
}