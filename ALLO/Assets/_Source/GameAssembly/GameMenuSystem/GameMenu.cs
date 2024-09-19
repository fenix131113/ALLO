using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameMenuSystem
{
	public class GameMenu : MonoBehaviour
	{
		[SerializeField] private GameObject gameMenuBlocker;
		[SerializeField] private GameObject settingsPanel;
		[SerializeField] private Button continueGameButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button menuButton;
		[SerializeField] private Button exitButton;

		public event Action OnContinue;
		private void Bind()
		{
			continueGameButton.onClick.AddListener(DeactivateMenu);
			settingsButton.onClick.AddListener(SwitchSettingsMenu);
			menuButton.onClick.AddListener(GoToMenu);
			exitButton.onClick.AddListener(ExitGame);
		}

		private void Expose()
		{
			continueGameButton.onClick.RemoveAllListeners();
			settingsButton.onClick.RemoveAllListeners();
			menuButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
		}

		public void SwitchMenu(bool status)
		{
			if (status)
				ActivateMenu();
			else
				DeactivateMenu();
		}

		private void SwitchSettingsMenu()
		{
			settingsPanel.SetActive(!settingsPanel.activeSelf);
		}

		private void ActivateMenu()
		{
			Cursor.visible = true;
			gameMenuBlocker.SetActive(true);
			Time.timeScale = 0;
			Bind();
		}

		private void DeactivateMenu()
		{
			gameMenuBlocker.SetActive(false);
			settingsPanel.SetActive(false);
			Cursor.visible = false;
			Time.timeScale = 1;
			OnContinue?.Invoke();
			Expose();
		}

		private void GoToMenu()
		{
			SceneManager.LoadScene(0);
			Expose();
		}

		private void ExitGame()
		{
			Application.Quit();
			Expose();
		}
	}
}