using System.Linq;
using LevelGenerationSystem.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGenerationSystem
{
    public class RandomSegment : MonoBehaviour
    {
        [field: SerializeField] public LevelSegmentRandomGroup[] UpRandomGroup { get; private set; }
        [field: SerializeField] public LevelSegmentRandomGroup[] RightRandomGroup { get; private set; }
        [field: SerializeField] public LevelSegmentRandomGroup[] DownRandomGroup { get; private set; }
        [field: SerializeField] public LevelSegmentRandomGroup[] LeftRandomGroup { get; private set; }
        [field: SerializeField] public LevelSegmentRandomGroup[] NonDoorsGorup { get; private set; }

        public void Generate(LevelSegmentRandomGroup[] randomGroup)
        {
            if (randomGroup.Length == 0)
                return;

            DeactivateAllVariants(randomGroup);

            foreach (var group in randomGroup)
            {
                var weightSum = group.Variants.Sum(variant => variant.Weight);

                foreach (var variant in group.Variants)
                {
                    if (Random.Range(0, weightSum + 1) > variant.Weight)
                    {
                        weightSum -= variant.Weight;
                        continue;
                    }

                    if (variant.Variants.Length > 0)
                        foreach (var item in variant.Variants)
                        {
                            if (item)
                                item.SetActive(true);
                        }

                    break;
                }
            }
        }

        private void DeactivateAllVariants(LevelSegmentRandomGroup[] randomGroup)
        {
            foreach (var group in randomGroup)
            foreach (var variant in group.Variants)
                if (variant.Variants.Length > 0)
                    foreach (var item in variant.Variants)
                    {
                        if (item)
                            item.SetActive(false);
                    }
        }
    }
}