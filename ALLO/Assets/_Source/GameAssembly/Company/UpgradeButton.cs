using System;
using PlayerSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Company
{
    public class UpgradeButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
        
        public UpgradesDataSO UpgradeData { get; private set; }
        
        public event Action<UpgradeButton> OnClicked;

        public void Init(UpgradesDataSO upgradeData, int nextLevel)
        {
            UpgradeData = upgradeData;
            upgradeNameText.text = upgradeData.UpgradeName;
            descriptionText.text = upgradeData.Description;
            iconImage.sprite = upgradeData.Icon;
            levelText.text =  $"Level: {nextLevel - 1} -> {nextLevel.ToString()}";
        }

        public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(this);
    }
}