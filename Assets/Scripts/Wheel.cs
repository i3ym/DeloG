using UnityEngine;

namespace DeloG
{
    [RequireComponent(typeof(WheelCollider))]
    public class Wheel : MonoBehaviour
    {
        public WheelCollider WheelCollider => Collider;
        public bool Rotatable => IsRotatable;

        [SerializeField] bool IsRotatable = false;
        WheelCollider Collider;
        float HorizontalAngle = 0f;

        void Start()
        {
            Collider = GetComponent<WheelCollider>();
        }

        public void SetInputs(float motorTorque, float brakeTorque, float horizontalAngle)
        {
            Collider.brakeTorque = brakeTorque;
            Collider.motorTorque = motorTorque;
            transform.Rotate(transform.right, Collider.rpm * 360 / 60 * Time.fixedDeltaTime, Space.World);

            if (Rotatable)
            {
                Collider.steerAngle = horizontalAngle;
                transform.Rotate(transform.parent.up, -(HorizontalAngle - horizontalAngle), Space.World);
                HorizontalAngle = horizontalAngle;
            }
        }
    }
}