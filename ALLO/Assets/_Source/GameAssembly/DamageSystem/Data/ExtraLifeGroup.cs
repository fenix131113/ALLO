using System;
using UnityEngine;

namespace DamageSystem.Data
{
	[Serializable]
	public class ExtraLifeGroup
	{
		[field: Range(0f, 1f)]
		[field: SerializeField] public float ExtraLifeChance { get; private set; }
	}
}