using UnityEngine;

namespace DeloG
{
    [RequireComponent(typeof(WheelCollider))]
    public class Wheel : MonoBehaviour
    {
        public WheelCollider WheelCollider => Collider;
        public bool Rotatable => IsRotatable;

        [SerializeField] MeshRenderer Mesh = null;
        [SerializeField] bool IsRotatable = false;
        WheelCollider Collider;
        float HorizontalAngle = 0f;

        void Start()
        {
            Collider = GetComponent<WheelCollider>();
        }

        void Update()
        {
            var center = transform.TransformPoint(Collider.center);
            var raycast = Physics.Raycast(center, -transform.parent.up, out var hit, Collider.suspensionDistance + Collider.radius);

            if (raycast) Mesh.transform.position = hit.point + (transform.parent.up * Collider.radius);
            else Mesh.transform.position = center - (transform.parent.up * Collider.suspensionDistance);
        }

        public void SetTorque(float motorTorque, float brakeTorque)
        {
            Collider.brakeTorque = brakeTorque;
            Collider.motorTorque = motorTorque;
        }
        public void SetAngle(float horizontalAngle)
        {
            Mesh.transform.Rotate(-Mesh.transform.right, Collider.rpm * 360 / 60 * Time.fixedDeltaTime, Space.World);

            if (Rotatable)
            {
                Collider.steerAngle = horizontalAngle;
                Mesh.transform.Rotate(transform.parent.up, -(HorizontalAngle - horizontalAngle), Space.World);
                HorizontalAngle = horizontalAngle;
            }
        }
        public void SetInputs(float motorTorque, float brakeTorque, float horizontalAngle)
        {
            SetTorque(motorTorque, brakeTorque);
            SetAngle(horizontalAngle);
        }
    }
}