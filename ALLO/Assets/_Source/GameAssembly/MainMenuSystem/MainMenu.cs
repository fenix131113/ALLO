using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenuSystem
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button startGameButton;
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
			exitButton.onClick.AddListener(Application.Quit);
		}

		private void Expose()
		{
			startGameButton.onClick.RemoveAllListeners();
			settingsButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
		}

		private void LoadGameScene()
		{
			SceneManager.LoadScene(1);
			Expose();
		}

		private void SwitchSettingsPanel()
		{
			settingsPanel.SetActive(!settingsPanel.activeSelf);
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}
