using DamageSystem;
using DamageSystem.Data;
using UnityEngine;

namespace PlayerSystem.Attack.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private DamageOwner damageOwner;
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;
        [SerializeField] private int damage;
        
        private bool _isKill;

        private void Start() => Destroy(gameObject, lifetime);

        private void Update() => transform.position += transform.right * (speed * Time.deltaTime);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable) && damageable.GetOwner() != damageOwner && !_isKill)
            {
                damageable.TakeDamage(damage);
                _isKill = true;
                
                Destroy(gameObject);
            }
            else if (damageable == null)
                Destroy(gameObject); //TODO: Deactivate instead destroying, use dynamic object pool
        }

        public void SetDamageOwner(DamageOwner owner) => damageOwner = owner;
        public void SetDamage(int dmg) => damage = dmg;
    }
}