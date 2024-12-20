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
		[SerializeField] private int generationSceneIndex;
		[SerializeField] private int arenaSceneIndex;

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
			SceneManager.LoadScene(arenaSceneIndex);
		}
		
		private void LoadGenerationScene()
		{
			Expose();
			SceneManager.LoadScene(generationSceneIndex);
		}

		private void SwitchSettingsPanel()
		{
			settingsPanel.SetActive(!settingsPanel.activeSelf);
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}
