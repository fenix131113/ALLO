using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Shooting.Data;
using UnityEngine;
using Utils;

namespace PlayerSystem.Items
{
    public class CollectableAmmoBox : MonoBehaviour //TODO: Replace with multi-taking logic
    {
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private AmmoType ammoType;
        [SerializeField] private int ammoAmount;

        private PlayerAmmoContainer _playerAmmoContainer;
        private bool _isGetAmmo;

        public void Init(PlayerAmmoContainer playerAmmoContainer)
        {
            _playerAmmoContainer = playerAmmoContainer;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, playerLayerMask) &&
                other.TryGetComponent(out Player _))
                return;

            _playerAmmoContainer.TryChangeAmmoInStorage(ammoType, ammoAmount);

            Destroy(gameObject);
        }
    }
}