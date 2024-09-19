using UnityEngine;

namespace Utils
{
	public static class ParticleSystemExtension
	{
		public static void DestroyByTime(this ParticleSystem target, float time) => Object.Destroy(target.gameObject, time);
	}
}