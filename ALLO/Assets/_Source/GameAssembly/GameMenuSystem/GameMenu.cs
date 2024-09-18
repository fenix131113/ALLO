using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenuSystem
{
	public class GameMenu : MonoBehaviour
	{
		[SerializeField] private GameObject gameMenuBlocker;
		[SerializeField] private Button continueGameButton;

		public event Action OnContinue; 

		public void SwitchMenu(bool status)
		{
			if (status)
				ActivateMenu();
			else
				DeactivateMenu();
		}

		private void ActivateMenu()
		{
			Cursor.visible = true;
			gameMenuBlocker.SetActive(true);
			Time.timeScale = 0;
			continueGameButton.onClick.AddListener(DeactivateMenu);
		}

		private void DeactivateMenu()
		{
			Cursor.visible = false;
			gameMenuBlocker.SetActive(false);
			Time.timeScale = 1;
			continueGameButton.onClick.RemoveAllListeners();
			OnContinue?.Invoke();
		}
	}
}