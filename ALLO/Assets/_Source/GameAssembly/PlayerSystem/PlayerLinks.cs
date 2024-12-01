using PlayerSystem.Attack;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    //TODO: Replace with zenject fabric
    public class PlayerLinks : MonoBehaviour
    {
        public PlayerWeaponsData PlayerWeaponsData { get; private set; }

        [Inject]
        private void Construct(PlayerWeaponsData playerWeaponsData)
        {
            PlayerWeaponsData = playerWeaponsData;
        }
    }
}
