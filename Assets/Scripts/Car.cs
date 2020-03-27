using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public class Car : MonoBehaviour
    {
        const float MaxWheelAngle = 45; // максимальный угол поворота колёс
        const float SteeringWheelMultiplier = 6; // умножение кручение руля

        public event Action<bool> OnChangeState = delegate { };

        [SerializeField] float MotorTorque = 500; // скорость
        float MotorTorqueFast => MotorTorque * 1.5f; // скорость на шифт

        [SerializeField] public Transform PlayerPosition = null;
        [SerializeField] Transform SteeringWheel = null;
        IReadOnlyCollection<Wheel> Wheels;
        Camera Camera;

        public bool IsHandbrakeOn = false;
        public bool IsTurnedOn
        {
            get => _IsTurnedOn;
            set => OnChangeState(_IsTurnedOn = value);
        }
        public bool Enabled = false;
        [HideInInspector] bool _IsTurnedOn = false;

        float SteeringWheelAngle = 0f;
        Vector3 StartSteeringWheelRotation;

        public void Start()
        {
            Wheels = GetComponentsInChildren<Wheel>();
            Camera = Camera.main;

            foreach (var wheel in Wheels)
                wheel.WheelCollider.ConfigureVehicleSubsteps(5, 12, 15);

            StartSteeringWheelRotation = SteeringWheel.localEulerAngles;
        }

        void FixedUpdate() => DoMovement();
        void DoMovement()
        {
            if (!Enabled)
            {
                foreach (var wheel in Wheels)
                    wheel.SetInputs(0, 0, 0);

                return;
            }

            var angle = Input.GetAxis("Horizontal") * MaxWheelAngle;
            var steerangle = angle * SteeringWheelMultiplier;
            SteeringWheel.Rotate(SteeringWheel.up, -(SteeringWheelAngle - steerangle), Space.World);
            SteeringWheelAngle = steerangle;

            float torque = 0;
            float brake = 0;

            if (IsTurnedOn)
            {
                var isbrake = Input.GetKey(KeyCode.Space);
                var isfast = Input.GetKey(KeyCode.LeftShift);

                var speed = isfast ? MotorTorqueFast : MotorTorque;
                brake = isbrake ? speed * 5 : 0;
                torque = isbrake ? 0 : Input.GetAxis("Vertical") * speed;
            }
            if (IsHandbrakeOn) brake = MotorTorqueFast * 10_000; // сила остановки ручником

            foreach (var wheel in Wheels)
                wheel.SetInputs(torque, brake, angle);
        }

        void OnDisable()
        {
            foreach (var wheel in Wheels)
                wheel.SetInputs(0, 0, 0);
        }
    }
}