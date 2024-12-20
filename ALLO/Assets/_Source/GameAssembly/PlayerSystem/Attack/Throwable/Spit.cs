using System;
using System.Linq;
using DamageSystem;
using DamageSystem.Data;
using UnityEngine;
using Utils;

namespace PlayerSystem.Attack.Throwable
{
    public class Spit : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private DamageOwner damageOwner;
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;
        [SerializeField] private float explodeRadius;
        [SerializeField] private ParticleSystem explodeParticles;

        private void FixedUpdate() => transform.position += transform.right * (speed * Time.deltaTime);

        private void Start() => Destroy(gameObject, lifetime);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayer))
                return;
            
            var damageableList = Physics2D.CircleCastAll(transform.position, explodeRadius, transform.up)
                .Select(find =>
                {
                    find.transform.TryGetComponent(out IDamageable damageable);
                    return damageable;
                }).Where(t => t != null).ToList();

            foreach (var damageable in damageableList.Where(damageable => damageable.GetOwner() != damageOwner))
                damageable.TakeDamage(damage);

            // Effect
            explodeParticles.Play();
            explodeParticles.transform.parent = null;
            explodeParticles.transform.localScale = Vector3.one;
            explodeParticles.DestroyByTime(explodeParticles.main.duration);
            
            Destroy(gameObject); //TODO: Dynamic object pool
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explodeRadius);
        }
    }
}