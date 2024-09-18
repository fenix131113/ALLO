using DamageSystem;
using DamageSystem.Data;
using Pathfinding;
using UnityEngine;

namespace EnemySystem
{
	public abstract class AEnemy : MonoBehaviour, IDamageable
	{
		[field: SerializeField] public int Health { get; protected set; }
		[field: SerializeField] public int MaxHealth { get; protected set; }
		[field: SerializeField] public EnemyVision Vision { get; protected set; }
		[field: SerializeField] public AIPath AiPath { get; protected set; }
		[field: SerializeField] public DamageOwner Owner { get; protected set; }

		private void Start()
		{
			Bind();
		}

		protected abstract void OnTargetSpotted(Transform target);
		
		public int GetHealth() => Health;
		public int GetMaxHealth() => Health;
		public DamageOwner GetOwner() => Owner;

		public void TakeDamage(int damage)
		{
			Health -= damage;
			Health = Mathf.Clamp(Health, 0, MaxHealth);
		}

		public void SetDestination(Vector3 targetPosition)
		{
			AiPath.destination = targetPosition;
		}

		private void Bind()
		{
			Vision.OnTargetSpotted += OnTargetSpotted;
		}

		protected void Expose()
		{
			Vision.OnTargetSpotted -= OnTargetSpotted;
		}

		private void OnDestroy() => Expose();
		private void OnApplicationQuit() => Expose();
	}
}