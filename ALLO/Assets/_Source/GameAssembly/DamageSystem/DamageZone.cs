using System.Collections;
using System.Collections.Generic;
using DamageSystem.Data;
using UnityEngine;

namespace DamageSystem
{
	public class DamageZone : MonoBehaviour
	{
		[Tooltip("Disable zone after giving damage\nOtherwise every entry takes damage only once")] [SerializeField]
		private bool instantDamage;

		[SerializeField] private bool deactivatingByTime;
		[SerializeField] private float deactivationTime;
		[SerializeField] private DamageOwner damageOwner;
		[SerializeField] private int damage = 1;

		private readonly Dictionary<GameObject, IDamageable> _damageableInside = new();

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

		public void ActivateZone()
		{
			gameObject.SetActive(true);

			if (deactivatingByTime)
				StartCoroutine(DeactivateZoneByTime());
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (instantDamage)
			{
				if (!other.gameObject.TryGetComponent(out IDamageable instantDamageable)) return;
				if(instantDamageable.GetOwner() != damageOwner) return;
				
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

		private IEnumerator DeactivateZoneByTime()
		{
			yield return new WaitForSeconds(deactivationTime);

			DisableZone();
		}
	}
}