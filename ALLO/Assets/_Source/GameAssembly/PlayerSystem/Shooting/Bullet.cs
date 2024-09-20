using DamageSystem;
using UnityEngine;

namespace PlayerSystem.Shooting
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float speed;
		[SerializeField] private float lifetime;
		[SerializeField] private int damage;

		private bool _isKill;

		private void Start()
		{
			Destroy(gameObject, lifetime);
		}

		private void Update()
		{
			transform.position += transform.right * (speed * Time.deltaTime);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out IDamageable damageable) && !_isKill)
			{
				damageable.TakeDamage(damage);
				_isKill = true;
			}

			Destroy(gameObject); //TODO: Deactivate instead destroying, use dynamic object pool
		}
	}
}