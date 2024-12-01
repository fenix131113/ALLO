using PlayerSystem.Attack.Data;
using UnityEngine;
using Utils;

namespace PlayerSystem.Items.Collectable
{
    public class WeaponCollectable : MonoBehaviour
    {
        [field: SerializeField] private WeaponBaseDataSO weapon;
        [field: SerializeField] private LayerMask interactionLayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactionLayer))
                return;

            if (!other.TryGetComponent(out PlayerLinks playerLinks))
                return;
            
            playerLinks.PlayerWeaponsData.TryAddWeapon(weapon);
            Destroy(gameObject);
        }
    }
}