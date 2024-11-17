using UnityEngine;

namespace LevelGenerationSystem.Data
{
    [CreateAssetMenu(fileName = "New GenerationSettings", menuName = "Configs/Level Generator/New Generating Settings")]
    public class GenerationSettingsSO : ScriptableObject
    {
        [field: SerializeField] public int StartSegmentsXCount { get; private set; }
        [field: SerializeField] public int StartSegmentsYCount { get; private set; }
        [field: SerializeField] public LevelSegmentSO StartSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO EndSegment { get; private set; }
    }
}