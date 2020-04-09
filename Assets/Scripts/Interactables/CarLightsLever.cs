using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class CarLightsLever : Lever
    {
        [SerializeField] Light[] Lights = null;

        protected override void Awake()
        {
            base.Awake();
            ToggleLights(false);

            Car.OnChangeState += (on) =>
            {
                if (IsToggled)
                    foreach (var light in Lights)
                        light.enabled = on;
            };
        }

        protected override void OnToggle(bool toggled) { }
        protected void ToggleLights(bool enabled)
        {
            foreach (var light in Lights)
                light.enabled = enabled && Car.IsTurnedOn;
        }

        protected override IEnumerator ToggleAnimation() => null;
        protected override IEnumerator UntoggleAnimation() => null;
    }
}