using System;
using System.Collections.Generic;
using System.Linq;
using PlayerSystem;
using PlayerSystem.Items;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Security = EnemySystem.Enemies.Security;

namespace WavesSystem
{
	public class Waves : MonoBehaviour
	{
		[SerializeField] private CollectableAmmoBox ammoBoxPrefab;
		[SerializeField] private float safeSpawnDistance;
		[SerializeField] private List<Transform> spawnPoints = new();
		[SerializeField] private Security enemyPrefab;

		public int Wave { get; private set; }
		public int EnemyLeft => _spawnedEnemies.Count;

		private int _enemyWaveCount = 0;
		private List<Security> _spawnedEnemies = new();
		private PlayerMutation _playerMutation;

		public event Action OnEnemyCountChanged;

		[Inject]
		private void Construct(PlayerMutation playerMutation)
		{
			_playerMutation = playerMutation;
		}

		private void NextWave()
		{
			Wave++;
			if (Wave % 2 == 0)
				_enemyWaveCount = (int)(_enemyWaveCount * 1.5f);
			else
				_enemyWaveCount++;
			
			var availableSpawnPoint = spawnPoints.Where(point =>
				Vector3.Distance(point.position, _playerMutation.CurrentPlayer.transform.position) >
				safeSpawnDistance).ToList();
				
			var spawned = Instantiate(ammoBoxPrefab, availableSpawnPoint.ElementAt(Random.Range(0, availableSpawnPoint.Count())).position,
				Quaternion.identity);

			SpawnEnemies();
		}

		private void SpawnEnemies()
		{
			for (var i = 0; i < _enemyWaveCount; i++)
			{
				var availableSpawnPoint = spawnPoints.Where(point =>
					Vector3.Distance(point.position, _playerMutation.CurrentPlayer.transform.position) >
					safeSpawnDistance).ToList();
				
				var spawned = Instantiate(enemyPrefab, availableSpawnPoint.ElementAt(Random.Range(0, availableSpawnPoint.Count())).position,
					Quaternion.identity);
				spawned.Vision.NativeSetTarget(_playerMutation.CurrentPlayer.transform);
				_spawnedEnemies.Add(spawned);
			}
			
			OnEnemyCountChanged?.Invoke();
		}

		private void Update()
		{
			if (_spawnedEnemies.Count == 0)
				NextWave();
		}

		private void FixedUpdate()
		{
			if (_spawnedEnemies.Count <= 0) return;

			foreach (var enemy in _spawnedEnemies.Where(enemy => enemy == null))
			{
				_spawnedEnemies.Remove(enemy);
				OnEnemyCountChanged?.Invoke();
				break;
			}
		}
	}
}