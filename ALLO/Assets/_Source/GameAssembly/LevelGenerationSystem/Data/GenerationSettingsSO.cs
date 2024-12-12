using UnityEngine;

namespace LevelGenerationSystem.Data
{
    [CreateAssetMenu(fileName = "New GenerationSettings", menuName = "Configs/Level Generator/New Generating Settings")]
    public class GenerationSettingsSO : ScriptableObject
    {
        [field: SerializeField] public int StartSegmentsXCount { get; private set; }
        [field: SerializeField] public int StartSegmentsYCount { get; private set; }

        [field: Range(0, 60)]
        [field: SerializeField]
        public int CorridorPercent { get; private set; }

        [field: SerializeField] public LevelSegmentSO StartSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO EndSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO XCorridorSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO TCorridorSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO TCorridorSegment90 { get; private set; }
        [field: SerializeField] public LevelSegmentSO TCorridorSegment180 { get; private set; }
        [field: SerializeField] public LevelSegmentSO TCorridorSegment270 { get; private set; }
        [field: SerializeField] public LevelSegmentSO LineCorridorSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO LineCorridorSegment90 { get; private set; }
        [field: SerializeField] public LevelSegmentSO CornerCorridorSegment { get; private set; }
        [field: SerializeField] public LevelSegmentSO CornerCorridorSegment90 { get; private set; }
        [field: SerializeField] public LevelSegmentSO CornerCorridorSegment180 { get; private set; }
        [field: SerializeField] public LevelSegmentSO CornerCorridorSegment270 { get; private set; }
    }
}