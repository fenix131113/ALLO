using PlayerSystem;
using UnityEngine;
using Utils;
using Zenject;

namespace Environment
{
    public class SwitchToMutantTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayer;
        
        private bool _isSwitched;
        private PlayerMutation _playerMutation;

        [Inject]
        private void Construct(PlayerMutation playerMutation) => _playerMutation = playerMutation;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayer) || _isSwitched)
                return;

            _isSwitched = true;
            _playerMutation.SwitchToMutantInstantly();
        }
    }
}