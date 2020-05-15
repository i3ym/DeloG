using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class Handbrake : RotateLever
    {
        protected override Quaternion ToggledRotation { get; } = Quaternion.Euler(-80, 0, 0);
        protected override Func<float, float> ToggleEasing { get; } = Easing.OutQuad;
        protected override Func<float, float> UntoggleEasing { get; } = Easing.OutQuad;
        protected override float AnimTime { get; } = .4f;

        protected override void Awake()
        {
            base.Awake();

            Toggle(true);
            Car.OnPlayerJoinExit += (join) =>
            {
                if (!join) Toggle(true);
            };
        }

        protected override void OnToggle(bool toggled) => Car.IsHandbrakeOn = toggled;
    }
}