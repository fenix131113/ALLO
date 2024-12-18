using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGenerationSystem
{
    public class RandomEnvironmentSpriteSelector : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer currentRenderer;
        [SerializeField] private RandomEnvironmentGroup[] spritesList;

        private void Start()
        {
            var weightSum = spritesList.Sum(sprite => sprite.Weight);

            foreach (var group in spritesList)
            {
                if (Random.Range(0, weightSum + 1) <= group.Weight)
                {
                    currentRenderer.sprite = group.Sprite;
                    break;
                }
                
                if (group == spritesList[^1])
                    currentRenderer.sprite = group.Sprite;
            }
        }

        [Serializable]
        public class RandomEnvironmentGroup
        {
            [field: SerializeField] public Sprite Sprite { get; private set; }
            [field: SerializeField] public int Weight { get; private set; }
        }
    }
}