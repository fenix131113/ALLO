using DamageSystem.Data;

namespace DamageSystem
{
	public interface IDamageable
	{
		public int GetHealth();
		public int GetMaxHealth();
		public void TakeDamage(int damage);
		public DamageOwner GetOwner();
	}
}