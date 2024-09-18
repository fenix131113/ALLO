using System.Collections.Generic;
using DamageSystem.Data;
using UnityEngine;

namespace DamageSystem
{
	public class DamageZone : MonoBehaviour
	{
		[Tooltip("Disable zone after giving damage\nOtherwise every entry takes damage only once")] [SerializeField]
		private bool instantDamage;

		private readonly Dictionary<GameObject, IDamageable> _damageableInside = new();
		[SerializeField] private DamageOwner damageOwner;
		[SerializeField] private int damage = 1;

		public void SetDamage(DamageOwner newDamageOwner, int newDamage)
		{
			damageOwner = newDamageOwner;
			damage = newDamage;
		}
		
		public void DisableZone()
		{
			gameObject.SetActive(false);
			_damageableInside.Clear();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (instantDamage)
			{
				if (!TryGetComponent(out IDamageable instantDamageable)) return;

				instantDamageable.TakeDamage(damage);
				DisableZone();
			}
			else
			{
				if (_damageableInside.ContainsKey(other.gameObject) ||
				    !other.TryGetComponent(out IDamageable damageable)) return;

				if (damageable.GetOwner() == damageOwner)
					damageable.TakeDamage(damage);
				_damageableInside.Add(other.gameObject, damageable);
			}
		}
	}
}