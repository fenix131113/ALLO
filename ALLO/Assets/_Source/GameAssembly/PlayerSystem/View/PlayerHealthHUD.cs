using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

namespace PlayerSystem.View
{
	public class PlayerHealthHUD : MonoBehaviour
	{
		[SerializeField] private Color downHealthColor;
		[SerializeField] private Color increaseHealthColor;
		[SerializeField] private float redHealthAnimTime;
		[SerializeField] private float downProgressInterval;
		[SerializeField] private float healthAnimTime;
		[SerializeField] private Image healthFiller;
		[SerializeField] private Image healthProgressFiller;
		[SerializeField] private TMP_Text healthLabel;

		private Player _player;
		private int lastHealth;

		[Inject]
		private void Construct(Player player)
		{
			_player = player;
		}

		private void Start()
		{
			Bind();

			lastHealth = _player.Health;
		}

		private void Bind()
		{
			_player.OnHealthChanged += Redraw;
		}

		private void Expose()
		{
			_player.OnHealthChanged -= Redraw;
		}

		private void Redraw() //TODO: Delete code repeating
		{
			if (_player.Health > lastHealth)
			{
				healthProgressFiller.color = increaseHealthColor;
				
				var seq = DOTween.Sequence();
				var fillAmount = (float)_player.Health / _player.MaxHealth;
				seq.Append(healthProgressFiller.DOFillAmount(fillAmount, healthAnimTime));
				seq.AppendInterval(downProgressInterval);
				seq.Append(healthFiller.DOFillAmount(fillAmount, redHealthAnimTime));
			}
			else
			{
				healthProgressFiller.color = downHealthColor;
				
				var seq = DOTween.Sequence();
				var fillAmount = (float)_player.Health / _player.MaxHealth;
				seq.Append(healthFiller.DOFillAmount(fillAmount, redHealthAnimTime));
				seq.AppendInterval(downProgressInterval);
				seq.Append(healthProgressFiller.DOFillAmount(fillAmount, healthAnimTime));
			}

			healthLabel.text = _player.Health + "/" + _player.MaxHealth;

			lastHealth = _player.Health;
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}