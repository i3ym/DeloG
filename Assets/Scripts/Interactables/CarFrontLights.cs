using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class CarFrontLights : CarLightsLever
    {
        [SerializeField] Renderer[] Lamps = null;

        protected override void Awake()
        {
            base.Awake();

            foreach (var lamp in Lamps)
                lamp.material.SetColor("_EmissionColor", lamp.material.color);
        }

        protected override void ToggleLights(bool enabled)
        {
            base.ToggleLights(enabled);

            foreach (var lamp in Lamps)
            {
                if (enabled && Car.IsTurnedOn)
                    lamp.material.EnableKeyword("_EMISSION");
                else lamp.material.DisableKeyword("_EMISSION");
            }
        }

        protected override IEnumerator ToggleAnimation() =>
            Animator.RotateToLocal(transform, Quaternion.Euler(0, 0, 20), .2f, Easing.InOutQuad);

        protected override IEnumerator UntoggleAnimation() =>
            Animator.RotateToLocal(transform, Quaternion.identity, .2f, Easing.InOutQuad);
    }
}