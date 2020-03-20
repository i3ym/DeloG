using UnityEngine;

namespace DeloG.Interactables
{
    public class CarLightsLever : Lever
    {
        [SerializeField] Car Car = null;
        [SerializeField] Light[] Lights = null;

        protected override void Start()
        {
            base.Start();
            OnUntoggle();

            Car.OnChangeState += (on) =>
            {
                if (IsToggled)
                    foreach (var light in Lights)
                        light.enabled = on;
            };
        }

        protected override void OnToggle()
        {
            foreach (var light in Lights)
                light.enabled = Car.IsTurnedOn;
        }
        protected override void OnUntoggle()
        {
            foreach (var light in Lights)
                light.enabled = false;
        }
    }
}