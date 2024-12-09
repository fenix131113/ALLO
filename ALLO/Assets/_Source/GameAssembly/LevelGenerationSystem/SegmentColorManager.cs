using System.Collections.Generic;
using System.Linq;
using LevelGenerationSystem.Data;
using UnityEngine;

namespace LevelGenerationSystem
{
    public class SegmentColorManager : MonoBehaviour
    {
        [SerializeField] private List<ElementSpriteSelector> colorSelectors;

        public void SelectColor(LevelSpritesColor levelSpritesColor)
        {
            foreach (var elementSpriteSelector in colorSelectors)
                elementSpriteSelector.SwitchSprite(levelSpritesColor);
        }

#if UNITY_EDITOR
        [ContextMenu("Select child sprite selectors")]
        private void SelectChildSpriteSelectors()
        {
            colorSelectors = transform.GetComponentsInChildren<ElementSpriteSelector>().ToList();
        }
#endif
    }
}