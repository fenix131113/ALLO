using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DamageSystem.Data
{
	[CreateAssetMenu(fileName = "Extra Life Module", menuName = "Configs/Extra Life Module")]
	public class ExtraLifeModule : ScriptableObject
	{
		[SerializeField] private List<ExtraLifeGroup> extraLifeGroups = new();
		
		public IReadOnlyList<ExtraLifeGroup> ExtraLifeGroups => extraLifeGroups;

		public bool CanGetExtraLife(int groupIndex)
		{
			var findGroup = extraLifeGroups[groupIndex];

			if (findGroup != null)
				return Random.Range(0, 1f) <= findGroup.ExtraLifeChance;

			return false;
		}
	}
}