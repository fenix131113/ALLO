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

		private PlayerMutation _playerMutation;
		private int _lastHealth;

		[Inject]
		private void Construct(PlayerMutation player)
		{
			_playerMutation = player;
		}

		private void Start()
		{
			Bind();

			_lastHealth = _playerMutation.CurrentPlayer.Health;
		}

		private void Bind()
		{
			_playerMutation.OnHealthChanged += Redraw;
		}

		private void Expose()
		{
			_playerMutation.OnHealthChanged -= Redraw;
		}

		private void Redraw()
		{
			if (_playerMutation.CurrentPlayer.Health > _lastHealth)
			{
				healthProgressFiller.color = increaseHealthColor;
				
				var seq = DOTween.Sequence();
				var fillAmount = (float)_playerMutation.CurrentPlayer.Health / _playerMutation.CurrentPlayer.MaxHealth;
				seq.Append(healthProgressFiller.DOFillAmount(fillAmount, healthAnimTime));
				seq.AppendInterval(downProgressInterval);
				seq.Append(healthFiller.DOFillAmount(fillAmount, redHealthAnimTime));
			}
			else
			{
				healthProgressFiller.color = downHealthColor;
				
				var seq = DOTween.Sequence();
				var fillAmount = (float)_playerMutation.CurrentPlayer.Health / _playerMutation.CurrentPlayer.MaxHealth;
				seq.Append(healthFiller.DOFillAmount(fillAmount, redHealthAnimTime));
				seq.AppendInterval(downProgressInterval);
				seq.Append(healthProgressFiller.DOFillAmount(fillAmount, healthAnimTime));
			}

			healthLabel.text = _playerMutation.CurrentPlayer.Health + "/" + _playerMutation.CurrentPlayer.MaxHealth;

			_lastHealth = _playerMutation.CurrentPlayer.Health;
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}