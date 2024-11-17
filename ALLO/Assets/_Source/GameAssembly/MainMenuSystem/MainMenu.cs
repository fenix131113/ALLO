using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenuSystem
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button startGameButton;
		[SerializeField] private Button generationButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button exitButton;
		[SerializeField] private GameObject settingsPanel;

		private void Start()
		{
			Bind();
		}

		private void Bind()
		{
			startGameButton.onClick.AddListener(LoadGameScene);
			settingsButton.onClick.AddListener(SwitchSettingsPanel);
			generationButton.onClick.AddListener(LoadGenerationScene);
			exitButton.onClick.AddListener(Application.Quit);
		}

		private void Expose()
		{
			startGameButton.onClick.RemoveAllListeners();
			settingsButton.onClick.RemoveAllListeners();
			generationButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
		}

		private void LoadGameScene()
		{
			Expose();
			SceneManager.LoadScene(1);
		}
		
		private void LoadGenerationScene()
		{
			Expose();
			SceneManager.LoadScene(2);
		}

		private void SwitchSettingsPanel()
		{
			settingsPanel.SetActive(!settingsPanel.activeSelf);
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}
