using System;
using UnityEngine;

namespace LevelGenerationSystem.Data
{
    [Serializable]
    public class SegmentLevelGenerationChanceGroup
    {
        [field: SerializeField] public LevelSegmentSO Segment { get; private set; }
        [field: SerializeField] public int Weight { get; private set; } = 50;
    }
}