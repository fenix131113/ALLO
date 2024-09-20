using TMPro;
using UnityEngine;

namespace WavesSystem.View
{
	public class WavesHUD : MonoBehaviour
	{
		[SerializeField] private Waves waves;
		[SerializeField] private TMP_Text wavesLabel;

		private void DrawWaveInfo() => wavesLabel.text = $"Wave {waves.Wave} - {waves.EnemyLeft} Enemies";

		private void Start() => Bind();

		private void Bind() => waves.OnEnemyCountChanged += DrawWaveInfo;

		private void Expose() => waves.OnEnemyCountChanged -= DrawWaveInfo;

		private void OnDestroy() => Expose();
		private void OnApplicationQuit() => Expose();
	}
}