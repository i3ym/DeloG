using UnityEngine;

namespace DeloG.Interactables
{
    public class CarLightsLever : Lever
    {
        [SerializeField] Light[] Lights;

        protected override void Start()
        {
            base.Start();
            OnUntoggle();
        }

        protected override void OnToggle()
        {
            foreach (var light in Lights)
                light.enabled = true;
        }
        protected override void OnUntoggle()
        {
            foreach (var light in Lights)
                light.enabled = false;
        }
    }
}