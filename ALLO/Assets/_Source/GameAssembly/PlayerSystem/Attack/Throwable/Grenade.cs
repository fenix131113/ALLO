using System.Collections;
using System.Linq;
using DamageSystem;
using DamageSystem.Data;
using UnityEngine;
using Utils;

namespace PlayerSystem.Attack.Throwable
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private DamageOwner damageOwner;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float startSpeed;
        [SerializeField] private float explodeRadius;
        [SerializeField] private float explodeTime;
        [SerializeField] private int damage;
        [SerializeField] private ParticleSystem explodeParticles;

        private void Start()
        {
            rb.AddForce(transform.right * startSpeed, ForceMode2D.Impulse);

            StartCoroutine(ExplodeCoroutine());
        }

        private void Explode()
        {
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

            Destroy(gameObject);
        }

        private IEnumerator ExplodeCoroutine()
        {
            yield return new WaitForSeconds(explodeTime);

            Explode();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explodeRadius);
        }
    }
}