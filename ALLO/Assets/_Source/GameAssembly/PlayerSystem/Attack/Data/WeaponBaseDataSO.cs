using UnityEngine;

namespace PlayerSystem.Attack.Data
{
    public class WeaponBaseDataSO : ScriptableObject
    {
        [field: SerializeField] public WeaponType WeaponType { get; protected set; }
        [field: SerializeField] public string WeaponName { get; protected set; }
        
        public T GetCurrentWeaponSO<T>() where T : Object
        {
            if (typeof(T) == GetType())
                return this as T;
            
            throw new System.ArgumentException(
                $"Can't get SO with type \"{typeof(T).Name}\"! Current SO type is {GetType().Name}");
        }
    }
}