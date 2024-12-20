using PlayerSystem.Attack.Throwable;
using UnityEngine;
using Utils;
using Zenject;

namespace PlayerSystem.Items.Collectable
{
    public class CollectableGrenade : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayerMask;

        private PlayerThrowable _throwable;
        
        [Inject]
        private void Construct(PlayerThrowable throwable) => _throwable = throwable;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, playerLayerMask))
                return;

            _throwable.IncreaseGrenadesCount();
            
            Destroy(gameObject);
        }
    }
}