using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class CarLightsLever : Lever
    {
        [SerializeField] Light[] Lights = null;

        protected override void Start()
        {
            base.Start();
            OnToggle(false);

            Car.OnChangeState += (on) =>
            {
                if (IsToggled)
                    foreach (var light in Lights)
                        light.enabled = on;
            };
        }

        protected override void OnToggle(bool toggled)
        {
            foreach (var light in Lights)
                light.enabled = toggled && Car.IsTurnedOn;
        }

        protected override IEnumerator ToggleAnimation() => null;
        protected override IEnumerator UntoggleAnimation() => null;
    }
}