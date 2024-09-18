using System;
using UnityEngine;

namespace EntityDrawers.Humanoid.Data
{
	[Serializable]
	public class HumanoidBodyRotationGroup
	{
		[field: SerializeField] public Sprite Head { get; private set; }
		[field: SerializeField] public Sprite Body { get; private set; }
		[field: SerializeField] public Sprite Legs { get; private set; }
		[field: SerializeField] public float LegsBlendTreeX { get; private set; }
		[field: SerializeField] public float LegsBlendTreeY { get; private set; }
	}
}
