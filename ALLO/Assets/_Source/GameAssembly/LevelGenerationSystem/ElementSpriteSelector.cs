using System;
using LevelGenerationSystem.Data;
using UnityEngine;

namespace LevelGenerationSystem
{
    public class ElementSpriteSelector : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer currentRenderer;
        [SerializeField] private Sprite whiteSprite;
        [SerializeField] private Sprite redSprite;
        [SerializeField] private Sprite graySprite;
        [SerializeField] private Sprite yellowSprite;

        public void SwitchSprite(LevelSpritesColor levelSpritesColor)
        {
            currentRenderer.sprite = levelSpritesColor switch
            {
                LevelSpritesColor.WHITE => whiteSprite,
                LevelSpritesColor.RED => redSprite,
                LevelSpritesColor.GRAY => graySprite,
                LevelSpritesColor.YELLOW => yellowSprite,
                _ => throw new ArgumentOutOfRangeException(nameof(levelSpritesColor), levelSpritesColor, null)
            };
        }
    }
}