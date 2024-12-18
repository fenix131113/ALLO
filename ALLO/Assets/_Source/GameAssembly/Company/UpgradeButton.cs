using System;
using PlayerSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Company
{
    public class UpgradeButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text upgradeNameText;
        
        public UpgradesDataSO UpgradeData { get; private set; }

        public event Action<UpgradeButton> OnClicked;

        public void Init(UpgradesDataSO upgradeData)
        {
            UpgradeData = upgradeData;
            upgradeNameText.text = upgradeData.UpgradeName;
        }

        public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(this);
    }
}