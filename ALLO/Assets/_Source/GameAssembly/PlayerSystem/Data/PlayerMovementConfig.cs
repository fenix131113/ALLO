using UnityEngine;

namespace PlayerSystem.Data
{
    [CreateAssetMenu(fileName = "New Player Config", menuName = "Configs/New Player Config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
        [field: Header("Movement")]
        [field: SerializeField] public float PlayerWalkSpeed { get; private set; }
        [field: SerializeField] public float PlayerRunSpeed { get; private set; }
        [field: SerializeField] public float PlayerDashPower { get; private set; }
        [field: Tooltip("Dashing time. Can't move while this time")]
        [field: SerializeField] public float PlayerDashTime { get; private set; }
        [field: SerializeField] public float PlayerDashCooldown { get; private set; }
    }
}