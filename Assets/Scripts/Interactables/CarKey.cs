using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class CarKey : RotateLever
    {
        protected override Quaternion ToggledRotation { get; } = Quaternion.Euler(0, 0, 90);
        protected override Func<float, float> ToggleEasing { get; } = Easing.OutQuad;
        protected override Func<float, float> UntoggleEasing { get; } = Easing.OutQuad;
        protected override float AnimTime { get; } = .5f;

        protected override void OnToggle(bool toggled) => Car.IsTurnedOn = toggled;
    }
}