using UnityEngine;

namespace DeloG.Interactables
{
    public class Lamp : Lever
    {
        [SerializeField] Light Light = null;

        protected override void Start()
        {
            base.Start();
            OnUntoggle();
        }

        protected override void OnToggle() => Light.enabled = true;
        protected override void OnUntoggle() => Light.enabled = false;
    }
}