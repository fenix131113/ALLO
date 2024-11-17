using System;
using UnityEngine;

namespace LevelGenerationSystem.Data
{
    [Serializable]
    public class LevelSegmentRandomGroup
    {
        [field: SerializeField] public SpawnVariantChanceGroup[] Variants { get; private set; } = Array.Empty<SpawnVariantChanceGroup>();
    }
}