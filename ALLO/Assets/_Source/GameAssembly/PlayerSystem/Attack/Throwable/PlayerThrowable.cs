using System;
using UnityEngine;

namespace PlayerSystem.Attack.Throwable
{
    public class PlayerThrowable : MonoBehaviour
    {
        [SerializeField] private Grenade grenadePrefab;
        [SerializeField] private Transform throwPoint;
        
        public int GrenadesCount { get; private set; }

        public event Action OnThrowableCountChanged;

        public void Throw()
        {
            Instantiate(grenadePrefab, transform.position, throwPoint.rotation);
        }

        public void IncreaseGrenadesCount()
        {
            GrenadesCount++;
            OnThrowableCountChanged?.Invoke();
        }

        public void LoadData(int grenadesCount)
        {
            GrenadesCount = grenadesCount;
            OnThrowableCountChanged?.Invoke();
        }
    }
}