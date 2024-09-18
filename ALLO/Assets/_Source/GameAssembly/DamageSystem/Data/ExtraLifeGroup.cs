using System;
using UnityEngine;

namespace DamageSystem.Data
{
	[Serializable]
	public class ExtraLifeGroup
	{
		[field: SerializeField] public int HitNumber { get; private set; }
		[field: Range(0f, 1f)]
		[field: SerializeField] public float ExtraLifeChance { get; private set; }
	}
}