using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DamageSystem.Data
{
	[CreateAssetMenu(fileName = "Extra Life Module", menuName = "Configs/Extra Life Module")]
	public class ExtraLifeModule : ScriptableObject
	{
		[SerializeField] private List<ExtraLifeGroup> extraLifeGroups = new();

		public bool CanGetExtraLife(int hitCount)
		{
			var findGroup = extraLifeGroups.Find(group => group.HitNumber == hitCount);

			if (findGroup != null)
				return Random.Range(0, 1f) <= findGroup.ExtraLifeChance;

			return false;
		}
	}
}