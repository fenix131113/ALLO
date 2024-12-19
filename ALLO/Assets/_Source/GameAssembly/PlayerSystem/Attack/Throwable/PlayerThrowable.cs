using System;
using UnityEngine;

namespace PlayerSystem.Attack.Throwable
{
    public class PlayerThrowable : MonoBehaviour
    {
        [SerializeField] private Grenade grenadePrefab;
        [SerializeField] private Transform throwPoint;

        //TODO: Move to container script
        public int GrenadesCount { get; private set; }

        public event Action OnThrowableCountChanged;

        public void Throw()
        {
            if (GrenadesCount <= 0)
                return;
            
            Instantiate(grenadePrefab, transform.position, throwPoint.rotation);
            GrenadesCount--;
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