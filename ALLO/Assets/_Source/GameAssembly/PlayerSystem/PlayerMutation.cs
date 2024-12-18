using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerMutation : MonoBehaviour
    {
        [field: SerializeField] public Player DefaultPlayer { get; private set; }
        [field: SerializeField] public Player MutatedPlayer { get; private set; }
        [field: SerializeField] public int KillsToMutation { get; private set; }
        [SerializeField] public float mutatedMaxTime;
        
        public Player CurrentPlayer { get; private set; }
        public int ScoredKills{ get; private set; }

        public event Action OnPlayerKillsChanged;
        public event Action OnMutated;
        public event Action OnDead;
        public event Action OnHealthChanged;
        public event Action OnCompanyLevelComplete;

        [Inject]
        private void Construct(Player startPlayer)
        {
            CurrentPlayer = startPlayer;
        }

        private void Awake() => Bind();

        private void OnDestroy() => Expose();

        public void SetPlayer(Player newPlayer)
        {
            CurrentPlayer.gameObject.SetActive(false);
            newPlayer.transform.position = CurrentPlayer.transform.position;
            CurrentPlayer = newPlayer;
            CurrentPlayer.gameObject.SetActive(true);
            CurrentPlayer.OnMutated();
            OnMutated?.Invoke();
        }

        public void SwitchToMutant()
        {
            if (CurrentPlayer != DefaultPlayer || ScoredKills != KillsToMutation)
                return;
            
            SetPlayer(MutatedPlayer);
            ScoredKills = 0;
            OnPlayerKillsChanged?.Invoke();
            StartCoroutine(ReturnToDefaultPlayer());
        }

        public void IncreaseKillsCount()
        {
            ScoredKills = Mathf.Clamp(ScoredKills + 1, 0, KillsToMutation);
            OnPlayerKillsChanged?.Invoke();
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

        private IEnumerator ReturnToDefaultPlayer()
        {
            yield return new WaitForSeconds(mutatedMaxTime);
            
            SetPlayer(DefaultPlayer);
        }
    }
}