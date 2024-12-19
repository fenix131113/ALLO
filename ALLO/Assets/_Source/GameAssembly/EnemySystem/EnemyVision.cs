using System;
using UnityEngine;
using Utils;

namespace EnemySystem
{
    public class EnemyVision : MonoBehaviour
    {
        [SerializeField] private LayerMask targetingLayer;
        [SerializeField] private LayerMask visionLayers;
        [SerializeField] private Transform[] rayOutPoints;

        public Transform CurrentTarget { get; private set; }
        public bool IsPlayerInRange { get; private set; }
        public bool CanSeeTarget { get; private set; }
        
        public event Action<Transform> OnTargetSpotted;
        public event Action<Transform> OnTargetLost;
        public event Action<Transform> OnSeeTarget;

        private void Update()
        {
            if (!CurrentTarget)
                return;

            if (CanSeeTarget)
                OnSeeTarget?.Invoke(CurrentTarget);

            if (rayOutPoints.Length == 0)
            {
                var hit = Physics2D.Raycast(transform.position + Vector3.down * 0.1f,
                    CurrentTarget.transform.position + Vector3.down * 0.2f - transform.position, Mathf.Infinity,
                    visionLayers);

                if (!hit || hit.transform != CurrentTarget.transform)
                {
                    if (CanSeeTarget)
                        OnTargetLost?.Invoke(CurrentTarget.transform);

                    CanSeeTarget = false;
                    return;
                }
                
                CanSeeTarget = true;
                OnTargetSpotted?.Invoke(CurrentTarget.transform);
            }
            else
            {
                foreach (var point in rayOutPoints)
                {
                    var hit = Physics2D.Raycast(point.position,
                        CurrentTarget.transform.position - point.position, Mathf.Infinity,
                        visionLayers);

                    if (hit && hit.transform == CurrentTarget.transform)
                    {
                        CanSeeTarget = true;
                        OnTargetSpotted?.Invoke(CurrentTarget.transform);
                        break;
                    }
                    
                    if (point != rayOutPoints[^1])
                        continue;
                    
                    if (CanSeeTarget)
                        OnTargetLost?.Invoke(CurrentTarget.transform);

                    CanSeeTarget = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetingLayer))
                return;

            IsPlayerInRange = true;
            CurrentTarget = other.transform;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetingLayer) && CurrentTarget)
                return;

            IsPlayerInRange = false;
        }

        public void NativeSetTarget(Transform target, bool withEvent = true)
        {
            CurrentTarget = target;

            if (withEvent)
                OnTargetSpotted?.Invoke(target);
        }
    }
}