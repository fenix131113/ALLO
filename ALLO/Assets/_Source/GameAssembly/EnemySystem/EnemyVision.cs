using System;
using UnityEngine;
using Utils;

namespace EnemySystem
{
    public class EnemyVision : MonoBehaviour
    {
        [SerializeField] private LayerMask targetingLayer;
        [SerializeField] private LayerMask visionLayers;

        public Transform CurrentTarget { get; private set; }

        public event Action<Transform> OnTargetSpotted;

        private bool _isPlayerInRange;
        private Transform _playerTransform;

        private void Update()
        {
            if(!_isPlayerInRange)
                return;
            
            var hit = Physics2D.Raycast(transform.position,
                _playerTransform.transform.position - Vector3.down * 0.1f - transform.position, Mathf.Infinity, visionLayers);

            if (!hit || hit.transform != _playerTransform.transform)
                return;

            CurrentTarget = _playerTransform.transform;
            OnTargetSpotted?.Invoke(_playerTransform.transform);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetingLayer) || CurrentTarget)
                return;

            _isPlayerInRange = true;
            _playerTransform = other.transform;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetingLayer) || CurrentTarget)
                return;

            _isPlayerInRange = false;
        }

        public void NativeSetTarget(Transform target)
        {
            CurrentTarget = target;
            OnTargetSpotted?.Invoke(target);
        }
    }
}