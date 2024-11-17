using UnityEngine;
using Utils;

namespace PlayerSystem.Items
{
    public class CollectableFirstAid : MonoBehaviour //TODO: Replace with muliti-taking logic
    {
        [SerializeField] private LayerMask playerLayerMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, playerLayerMask))
                return;
            if (other.TryGetComponent<Player>(out var player))
            {
                player.AddHealth(10); //TODO: Delete GetComponent

                Destroy(gameObject);
            }
        }
    }
}