using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace PlayerSystem.View
{
	public class PlayerDeadHUD : MonoBehaviour
	{
		[SerializeField] private GameObject deadPanel;
		[SerializeField] private Button restartButton;
		[SerializeField] private Button menuButton;

		private PlayerMutation _playerMutation;

		[Inject]
		private void Construct(PlayerMutation playerMutation)
		{
			_playerMutation = playerMutation;
		}

		private void Start()
		{
			_playerMutation.CurrentPlayer.OnDead += ActivateDeadMenu;
		}

		private void Bind()
		{
			restartButton.onClick.AddListener(RestartGame);
			menuButton.onClick.AddListener(LoadMenu);
		}

		private void Expose()
		{
			_playerMutation.CurrentPlayer.OnDead -= ActivateDeadMenu;
			
			restartButton.onClick.RemoveAllListeners();
			menuButton.onClick.RemoveAllListeners();
		}

		private void ActivateDeadMenu()
		{
			Time.timeScale = 0;
			Cursor.visible = true;
			
			Bind();
			deadPanel.SetActive(true);
		}
		
		private void LoadMenu()
		{
			Expose();
			Time.timeScale = 1;
			SceneManager.LoadScene(0);
		}

		private static void RestartGame()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}