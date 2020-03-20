using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public class Car : MonoBehaviour
    {
        const float MaxWheelAngle = 45; // максимальный угол поворота колёс
        const float SteeringWheelMultiplier = 6; // умножение кручение руля

        [SerializeField] float MotorTorque = 500; // скорость
        float MotorTorqueFast => MotorTorque * 1.5f; // скорость на шифт

        [SerializeField] public Transform PlayerPosition = null;
        [SerializeField] Transform SteeringWheel = null;
        IReadOnlyCollection<Wheel> Wheels;
        Camera Camera;

        float SteeringWheelAngle = 0f;
        Vector3 StartSteeringWheelRotation;

        public void Start()
        {
            Wheels = GetComponentsInChildren<Wheel>();
            Camera = Camera.main;

            foreach (var wheel in Wheels)
                wheel.WheelCollider.ConfigureVehicleSubsteps(5, 12, 15);

            StartSteeringWheelRotation = SteeringWheel.localEulerAngles;
            enabled = false;
        }

        void FixedUpdate() => DoMovement();
        void DoMovement()
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

        void OnDisable()
        {
            foreach (var wheel in Wheels)
                wheel.SetInputs(0, 0, 0);
        }
    }
}