using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayerSystem.View
{
	public class PlayerDashHUD : MonoBehaviour
	{
		[SerializeField] private Color activeColor;
		[SerializeField] private Color deactivatedColor;
		[SerializeField] private Image dashFiller;

		private PlayerMovement _playerMovement;

		[Inject]
		private void Construct(PlayerMovement playerMovement) => _playerMovement = playerMovement;

		private void Update()
		{
			if (!_playerMovement.IsDashCooldown)
			{
				dashFiller.color = activeColor;
				dashFiller.fillAmount = 1f;
				return;
			}
			
			dashFiller.fillAmount = _playerMovement.DashCooldownProgress;
			dashFiller.color = deactivatedColor;
		}
	}
}