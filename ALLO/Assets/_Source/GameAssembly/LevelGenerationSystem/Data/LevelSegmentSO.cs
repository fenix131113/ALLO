using System.Linq;
using UnityEngine;

namespace LevelGenerationSystem.Data
{
    [CreateAssetMenu(fileName = "New LevelSegmentSO", menuName = "Configs/Level Generator/New Level Segment")]
    public class LevelSegmentSO : ScriptableObject
    {
        [field: SerializeField] public LevelSegment SegmentPrefab { get; private set; }
        [field: SerializeField] public SegmentLevelGenerationChanceGroup[] ConnectableTo { get; private set; }

        public LevelSegmentSO GetRandomNextLevelSegment()
        {
            LevelSegmentSO segmentToReturn = null;
            
            var weightSum = ConnectableTo.Sum(seg => seg.Weight);
            
            foreach (var segment in ConnectableTo)
            {
                if (Random.Range(0, weightSum + 1) <= segment.Weight)
                {
                    segmentToReturn = segment.Segment;
                    break;
                }

                weightSum -= segment.Weight;
            }
            
            return segmentToReturn;
        }
    }
}