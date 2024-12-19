using EntityDrawers.Humanoid.Data;
using UnityEngine;

namespace EntityDrawers.Humanoid
{
    public class DrawerBase : MonoBehaviour
    {
        [field: SerializeField] public Transform LookTarget { get; private set; }
        [field: SerializeField] public Transform CenterPoint { get; private set; }

        public Vector3? LookPosition { get; private set; }

        protected float GetLookDegrees()
        {
            Vector2 lookDirection = LookTarget
                ? LookTarget.position - CenterPoint.position
                : (Vector3)LookPosition - CenterPoint.position;

            return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        }

        public virtual void GlowEffect(float time)
        {
        }

        protected virtual void Rotate(HumanoidRotationsEnum rotation)
        {
        }

        public virtual void Rotate(float degrees)
        {
        }

        public virtual void SetCurrentMovement(Vector2 movementVector, bool run)
        {
        }

        public virtual void SetRunState(bool state)
        {
        }

        public virtual void SetMovementDirection(Vector2 movementVector)
        {
        }

        public void SetLookTarget(Transform target)
        {
            LookTarget = target;
            LookPosition = null;
        }

        public void SetLookTarget(Vector3 position)
        {
            LookTarget = null;
            LookPosition = position;
        }
    }
}