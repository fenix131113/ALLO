using UnityEngine;

namespace PlayerSystem.Data
{
    [CreateAssetMenu(fileName = "New Player Config", menuName = "Configs/New Player Config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [field: SerializeField] public float PlayerWalkSpeed { get; private set; }
        [field: SerializeField] public float PlayerRunSpeed { get; private set; }
    }
}