using System;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMutation : MonoBehaviour
	{
		[field: SerializeField] public Player DefaultPlayer { get; private set; }
		[field: SerializeField] public Player MutatedPlayer { get; private set; }
		
		public Player CurrentPlayer { get; private set; }
		
		public event Action OnMutated;
		public event Action OnDead;
		public event Action OnHealthChanged;
		public event Action OnCompanyLevelComplete;

		[Inject]
		private void Construct(Player startPlayer) => CurrentPlayer = startPlayer;

		private void Awake() => Bind();

		private void OnDestroy() => Expose();

		private void SetPlayer(Player newPlayer)
		{
			CurrentPlayer.gameObject.SetActive(false);
			newPlayer.transform.position = CurrentPlayer.transform.position;
			CurrentPlayer = newPlayer;
			CurrentPlayer.gameObject.SetActive(true);
			CurrentPlayer.OnMutated();
			OnMutated?.Invoke();
		}
		
		public void SwitchMutation()
		{
			SetPlayer(CurrentPlayer == DefaultPlayer ? MutatedPlayer : DefaultPlayer);
		}
		
		private void InvokeDeadEvent() => OnDead?.Invoke();
		private void InvokeHealthChangedEvent() => OnHealthChanged?.Invoke();
		private void InvokeCompanyLevelCompleteEvent() => OnCompanyLevelComplete?.Invoke();
		
		private void Bind()
		{
			DefaultPlayer.OnThisPlayerDead += InvokeDeadEvent;
			MutatedPlayer.OnThisPlayerDead += InvokeDeadEvent;
			DefaultPlayer.OnThisPlayerHealthChanged += InvokeHealthChangedEvent;
			MutatedPlayer.OnThisPlayerHealthChanged += InvokeHealthChangedEvent;
			DefaultPlayer.OnCompanyLevelCompleted += InvokeCompanyLevelCompleteEvent;
			MutatedPlayer.OnCompanyLevelCompleted += InvokeCompanyLevelCompleteEvent;
		}
		
		private void Expose()
		{
			DefaultPlayer.OnThisPlayerDead -= InvokeDeadEvent;
			MutatedPlayer.OnThisPlayerDead -= InvokeDeadEvent;
			DefaultPlayer.OnThisPlayerHealthChanged -= InvokeHealthChangedEvent;
			MutatedPlayer.OnThisPlayerHealthChanged -= InvokeHealthChangedEvent;
			DefaultPlayer.OnCompanyLevelCompleted -= InvokeCompanyLevelCompleteEvent;
			MutatedPlayer.OnCompanyLevelCompleted -= InvokeCompanyLevelCompleteEvent;
		}
	}
}