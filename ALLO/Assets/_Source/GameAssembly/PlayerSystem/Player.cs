using System;
using DamageSystem;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using UnityEngine;

namespace PlayerSystem
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Player : MonoBehaviour, IDamageable
	{
		[field: SerializeField] public int Health { get; private set; }
		[field: SerializeField] public int MaxHealth { get; private set; }
		[field: SerializeField] public DamageOwner Owner { get; private set; }
		[field: SerializeField] public Rigidbody2D Rb { get; private set; }
		[field: SerializeField] public Transform LookRotationPivot { get; private set; }
		[field: SerializeField] public HumanoidBodyDrawer BodyDrawer { get; private set; }
		[field: SerializeField] public Transform ShootPoint { get; private set; }
		
		[SerializeField] private float hitGlowTime;

		//TODO: Change this
		public event Action OnHealthChanged;
		public event Action OnDead;

		public void OnMutated()
		{
		}

		public int GetHealth() => Health;
		public int GetMaxHealth() => MaxHealth;
		public DamageOwner GetOwner() => Owner;

		public void AddHealth(int amount)
		{
			Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
			OnHealthChanged?.Invoke();
		}
		
		public void TakeDamage(int damage)
		{
			Health -= damage;
			Health = Mathf.Clamp(Health, 0, MaxHealth);
			BodyDrawer.GlowEffect(hitGlowTime);
			OnHealthChanged?.Invoke();
			
			
			if(Health == 0)
				Die();
		}

		private void Die() => OnDead?.Invoke();
	}
}