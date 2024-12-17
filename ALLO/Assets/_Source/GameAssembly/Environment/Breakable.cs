using DamageSystem;
using DamageSystem.Data;
using UnityEngine;
using Utils;

namespace Environment
{
    public class Breakable : MonoBehaviour, IDamageable
    {
        private const DamageOwner DAMAGE_OWNER = DamageOwner.ENVIRONMENT;

        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private ParticleSystem breakParticle;
        [SerializeField] private ParticleSystem damageParticle;

        public int GetHealth() => health;

        public int GetMaxHealth() => maxHealth;

        public DamageOwner GetOwner() => DAMAGE_OWNER;

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
                DestroyObject();
            else
                damageParticle.Play();
        }

        private void DestroyObject()
        {
            breakParticle.DestroyByTime(breakParticle.main.duration);
            breakParticle.Play();
            breakParticle.transform.parent = null;
            breakParticle.gameObject.transform.localScale = Vector3.one;
            Destroy(gameObject);
        }
    }
}