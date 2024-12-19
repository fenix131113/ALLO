using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGenerationSystem
{
    public class FloorsRandomizer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] floorsRenderers;
        [SerializeField] private Sprite[] floorSprites;

        private void Start()
        {
            var selected = floorSprites[Random.Range(0, floorSprites.Length)];
            foreach (var floor in floorsRenderers)
                floor.sprite = selected;
        }
    }
}