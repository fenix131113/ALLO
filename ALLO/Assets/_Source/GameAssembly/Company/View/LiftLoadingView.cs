using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Company.View
{
    public class LiftLoadingView : MonoBehaviour
    {
        [SerializeField] private RectTransform upgradePanel;
        [SerializeField] private UpgradeButton[] cards;
        [SerializeField] private Image fade;
        [SerializeField] private AnimationCurve upgradePanelDownAnimCurve;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float moveUpgradePanelDownTime;
        [SerializeField] private float moveUpgradePanelUpTime;

        private LiftLoading _liftLoading;
        private CompanyInterLevelDataContainer _companyData;

        [Inject]
        private void Construct(LiftLoading liftLoading, CompanyInterLevelDataContainer companyData)
        {
            _liftLoading = liftLoading;
            _companyData = companyData;
        }

        private void Start()
        {
            Draw();
            Bind();
        }

        private void OnDestroy() => Expose();

        private void Draw()
        {
            if (_liftLoading.FirstUpgrade == null && _liftLoading.SecondUpgrade == null &&
                _liftLoading.ThirdUpgrade == null)
            {
                MoveUpgradePanelUp();

                var seq = DOTween.Sequence();
                seq.Append(FadeOut());
                seq.Append(FadeIn());
                seq.onComplete += LiftLoading.LoadNextLevel;
            }
            else
                FadeOut().onComplete += MoveUpgradePanelDown;

            if (_liftLoading.FirstUpgrade != null)
                cards[0].Init(_liftLoading.FirstUpgrade,
                    _companyData.CurrentUpgradesLevels.GetValueOrDefault(_liftLoading.FirstUpgrade.UpgradeType, 1));
            else
                cards[0].gameObject.SetActive(false);

            if (_liftLoading.SecondUpgrade != null)
                cards[1].Init(_liftLoading.SecondUpgrade,
                    _companyData.CurrentUpgradesLevels.GetValueOrDefault(_liftLoading.SecondUpgrade.UpgradeType, 1));
            else
                cards[1].gameObject.SetActive(false);

            if (_liftLoading.ThirdUpgrade != null)
                cards[2].Init(_liftLoading.ThirdUpgrade,
                    _companyData.CurrentUpgradesLevels.GetValueOrDefault(_liftLoading.ThirdUpgrade.UpgradeType, 1));
            else
                cards[2].gameObject.SetActive(false);
        }

        private Tween FadeOut() => fade.DOFade(0f, fadeOutTime).SetEase(Ease.InQuart);
        private Tween FadeIn() => fade.DOFade(1.0f, fadeInTime);

        private void MoveUpgradePanelDown()
        {
            upgradePanel.DOMoveY((float)Screen.height / 2, moveUpgradePanelDownTime)
                .SetEase(upgradePanelDownAnimCurve);
        }

        private void MoveUpgradePanelUp()
        {
            upgradePanel.DOMoveY(Screen.height * 1.5f, moveUpgradePanelDownTime).SetEase(Ease.InBack);
        }

        private void OnUpgradeButtonClicked(UpgradeButton button)
        {
            Expose();
            FadeIn().onComplete += LiftLoading.LoadNextLevel;
            MoveUpgradePanelUp();

            if (!_companyData.CurrentUpgradesLevels.TryAdd(button.UpgradeData.UpgradeType, 1))
                _companyData.CurrentUpgradesLevels[button.UpgradeData.UpgradeType] += 1;
        }

        private void Bind()
        {
            foreach (var button in cards)
                button.OnClicked += OnUpgradeButtonClicked;
        }

        private void Expose()
        {
            foreach (var button in cards)
                button.OnClicked -= OnUpgradeButtonClicked;
        }
    }
}