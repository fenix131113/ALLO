using System;
using UnityEngine;

namespace LevelGenerationSystem.Data
{
    [Serializable]
    public class SpawnVariantChanceGroup
    {
        [field: SerializeField] public GameObject Variant { get; private set; }
        [field: SerializeField] public int Weight { get; private set; } = 50;
    }
}