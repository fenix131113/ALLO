using System;
using System.Linq;
using LevelGenerationSystem.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGenerationSystem
{
    public class RandomSegment : MonoBehaviour
    {
        [field: SerializeField] private LevelSegmentRandomGroup[] randomGroups = Array.Empty<LevelSegmentRandomGroup>();

        private void OnEnable()
        {
            Generate();
        }

        public void Generate()
        {
            if (randomGroups.Length == 0)
                return;

            DeactivateAllVariants();

            foreach (var group in randomGroups)
            {
                var weightSum = group.Variants.Sum(variant => variant.Weight);

                foreach (var variant in group.Variants)
                {
                    if (Random.Range(0, weightSum + 1) > variant.Weight)
                    {
                        weightSum -= variant.Weight;
                        continue;
                    }

                    if (variant.Variant)
                        variant.Variant.SetActive(true);
                    break;
                }
            }
        }

        private void DeactivateAllVariants()
        {
            foreach (var group in randomGroups)
            foreach (var variant in group.Variants)
                if (variant.Variant)
                    variant.Variant.SetActive(false);
        }
    }
}