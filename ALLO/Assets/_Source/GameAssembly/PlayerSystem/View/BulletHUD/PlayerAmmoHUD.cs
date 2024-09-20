using System;
using System.Collections.Generic;
using DG.Tweening;
using PlayerSystem.Shooting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace PlayerSystem.View.BulletHUD
{
	public class PlayerAmmoHUD : MonoBehaviour
	{
		[SerializeField] private float bulletsXOffset = 10f;
		[SerializeField] private float moveBulletUpTime = 0.1f;
		[SerializeField] private BulletUiItem uiBulletPrefab;
		[SerializeField] private RectTransform startBulletRect;
		[SerializeField] private Transform bulletsParent;
		[SerializeField] private TMP_Text ammoLabel;
		[SerializeField] private Image ammoReloadFiller;

		private readonly List<BulletUiItem> _currentBullets = new();
		private PlayerShoot _playerShoot;
		private bool _isReloadTimer;
		private float _reloadTimer;

		[Inject]
		private void Construct(PlayerShoot playerShoot)
		{
			_playerShoot = playerShoot;
		}

		private void DrawAmmoLabel() => ammoLabel.text = $"x{_playerShoot.Ammo}";

		private void FillClipWithAmmo(int count)
		{
			if (_currentBullets.Count >= count)
				return;

			for (var i = _currentBullets.Count; i < count; i++)
				SpawnNewBullet();
		}

		private void FillFullClip()
		{
			FillClipWithAmmo(_playerShoot.AmmoInClip);
		}

		private void SpawnNewBullet()
		{
			var nextPosition = GetNextBulletPosition();
			var spawnedBullet =
				Instantiate(uiBulletPrefab, bulletsParent); //TODO: Replace logic with dynamic object pool

			spawnedBullet.Rect.rotation = startBulletRect.rotation;
			spawnedBullet.Rect.anchoredPosition = nextPosition;
			spawnedBullet.Rect.anchoredPosition -= new Vector2(0, spawnedBullet.Rect.sizeDelta.y);
			spawnedBullet.Rect.DOAnchorPosY(startBulletRect.anchoredPosition.y, moveBulletUpTime);
			spawnedBullet.GetComponent<Image>().DOFade(1, moveBulletUpTime);

			_currentBullets.Add(spawnedBullet);
		}

		private void ThrowFirstBullet()
		{
			var current = _currentBullets[0];
			current.StartRotate();

			current.Rect.DOJump(new Vector2(Screen.width + 25f, Random.Range(0, Screen.height / 3 + 1)), 0.2f, 1,
				0.5f).onComplete += () => Destroy(current.gameObject);

			_currentBullets.RemoveAt(0);

			MoveBulletsRight();
		}

		private Vector3 GetNextBulletPosition() =>
			startBulletRect.anchoredPosition - new Vector2(startBulletRect.sizeDelta.y + bulletsXOffset, 0) *
			_currentBullets.Count;

		private void MoveBulletsRight()
		{
			if (_currentBullets.Count == 0)
				return;

			foreach (var item in _currentBullets)
				item.Rect.DOAnchorPosX(item.Rect.anchoredPosition.x + item.Rect.sizeDelta.y + bulletsXOffset, 0.05f);
		}

		private void Bind()
		{
			_playerShoot.OnShoot += ThrowFirstBullet;
			_playerShoot.OnReloaded += FillFullClip;
			_playerShoot.OnReloaded += DrawAmmoLabel;
			_playerShoot.OnAmmoChanged += DrawAmmoLabel;
			_playerShoot.OnStartReloading += StartReloadTimer;
		}

		private void Expose()
		{
			_playerShoot.OnShoot -= ThrowFirstBullet;
			_playerShoot.OnReloaded -= FillFullClip;
			_playerShoot.OnReloaded -= DrawAmmoLabel;
			_playerShoot.OnAmmoChanged -= DrawAmmoLabel;
			_playerShoot.OnStartReloading -= StartReloadTimer;
		}

		private void CheckReloadTimer()
		{
			if (!_isReloadTimer)
				return;
			
			ammoReloadFiller.fillAmount = (Time.time - _reloadTimer) / _playerShoot.CurrentWeapon.ReloadTime;

			if (!(Time.time - _reloadTimer >= _playerShoot.CurrentWeapon.ReloadTime)) return;
			
			ammoReloadFiller.fillAmount = 0;
			_isReloadTimer = false;
		}

		private void StartReloadTimer()
		{
			_reloadTimer = Time.time;
			_isReloadTimer = true;
		}

		private void Update()
		{
			CheckReloadTimer();
		}

		private void Start()
		{
			FillClipWithAmmo(_playerShoot.AmmoInClip);

			DrawAmmoLabel();
			
			Bind();
		}

		private void OnDestroy() => Expose();

		private void OnApplicationQuit() => Expose();
	}
}