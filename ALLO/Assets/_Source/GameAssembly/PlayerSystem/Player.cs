using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    }
}
