using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public class Car : MonoBehaviour
    {
        const float MotorTorque = 500; // скорость
        const float MotorTorqueFast = 800; // скорость на шифт
        const float MaxWheelAngle = 45; // максимальный угол поворота колёс
        const float SteeringWheelMultiplier = 6; // умножение кручение руля

        [SerializeField] public Transform PlayerPosition = null;
        [SerializeField] Transform SteeringWheel = null;
        IReadOnlyCollection<Wheel> Wheels;

        float SteeringWheelAngle = 0f;
        Vector3 StartSteeringWheelRotation;

        public void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("car");
            Wheels = GetComponentsInChildren<Wheel>();

            foreach (var wheel in Wheels)
                wheel.WheelCollider.ConfigureVehicleSubsteps(5, 12, 15);

            StartSteeringWheelRotation = SteeringWheel.localEulerAngles;
            enabled = false;
        }

        void FixedUpdate()
        {
            var isbrake = Input.GetKey(KeyCode.Space);
            var isfast = Input.GetKey(KeyCode.LeftShift);

            var speed = isfast ? MotorTorqueFast : MotorTorque;
            var brake = isbrake ? speed : 0;
            var torque = isbrake ? 0 : Input.GetAxis("Vertical") * speed;
            var angle = Input.GetAxis("Horizontal") * MaxWheelAngle;

            foreach (var wheel in Wheels)
                wheel.SetInputs(torque, brake, angle);

            var steerangle = angle * SteeringWheelMultiplier;
            SteeringWheel.Rotate(SteeringWheel.up, -(SteeringWheelAngle - steerangle), Space.World);
            SteeringWheelAngle = steerangle;
        }
    }
}