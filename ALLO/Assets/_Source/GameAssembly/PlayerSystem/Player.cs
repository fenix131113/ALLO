using System;
using DamageSystem;
using DamageSystem.Data;
using EntityDrawers.Humanoid;
using UnityEngine;
using Zenject;

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
		[field: SerializeField] public DrawerBase BodyDrawerBase { get; private set; }
		[field: SerializeField] public Transform ShootPoint { get; private set; }
		
		[SerializeField] private float hitGlowTime;
		
		public event Action OnThisPlayerHealthChanged;
		public event Action OnThisPlayerDead;
		public event Action OnCompanyLevelCompleted;
		
		private PlayerMovement _playerMovement;

		[Inject]
		private void Construct(PlayerMovement playerMovement) => _playerMovement = playerMovement;

		public void OnMutated()
		{
			
		}

		public int GetHealth() => Health;
		public int GetMaxHealth() => MaxHealth;
		public DamageOwner GetOwner() => Owner;

		public void OnLevelComplete() => OnCompanyLevelCompleted?.Invoke();

		public void LoadData(int health)
		{
			Health = health;
			OnThisPlayerHealthChanged?.Invoke();
		}

		public void SetMaxHealth(int maxHealth)
		{
			MaxHealth = maxHealth;
		}
		
		public void AddHealth(int amount)
		{
			Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
			OnThisPlayerHealthChanged?.Invoke();
		}
		
		public void TakeDamage(int damage)
		{
			if(_playerMovement.IsDashTimerProceed)
				return;
			
			Health -= damage;
			Health = Mathf.Clamp(Health, 0, MaxHealth);
			BodyDrawerBase.GlowEffect(hitGlowTime);
			OnThisPlayerHealthChanged?.Invoke();
			
			
			if(Health == 0)
				Die();
		}

		private void Die() => OnThisPlayerDead?.Invoke();

		public void MovePlayer(Vector2 movement, bool run, bool moveWithAnimator = true)
		{
			Rb.velocity = movement;
			
			if(moveWithAnimator)
				BodyDrawerBase.SetCurrentMovement(movement, run);
		}
	}
}