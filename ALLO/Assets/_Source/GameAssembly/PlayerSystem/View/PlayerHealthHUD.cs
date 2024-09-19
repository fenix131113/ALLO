using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

namespace PlayerSystem.View
{
    public class PlayerHealthHUD : MonoBehaviour
    {
        [SerializeField] private float redHealthAnimTime;
        [SerializeField] private float downProgressInterval;
        [SerializeField] private float yellowHealthAnimTime;
        [SerializeField] private Image healthFiller;
        [SerializeField] private Image healthDownProgressFiller;
        [SerializeField] private TMP_Text healthLabel;
        
        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void Start()
        {
            Bind();
        }

        private void Bind()
        {
            _player.OnHealthChanged += Redraw;
        }

        private void Expose()
        {
            _player.OnHealthChanged -= Redraw;
        }

        private void Redraw()
        {
            var seq = DOTween.Sequence();
            var fillAmount = (float)_player.Health / _player.MaxHealth;
            seq.Append(healthFiller.DOFillAmount(fillAmount, redHealthAnimTime));
            seq.AppendInterval(downProgressInterval);
            seq.Append(healthDownProgressFiller.DOFillAmount(fillAmount, yellowHealthAnimTime));
            healthLabel.text = _player.Health + "/" + _player.MaxHealth;
        }

        private void OnDestroy() => Expose();

        private void OnApplicationQuit() => Expose();
    }
}
