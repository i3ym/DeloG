using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class CarLamp : CarLightsLever
    {
        [SerializeField] Renderer Lamp = null;

        protected override void Awake()
        {
            base.Awake();
            Lamp.material.SetColor("_EmissionColor", Lamp.material.color);
        }

        protected override void OnToggle(bool toggled) { }
        protected override void ToggleLights(bool enabled)
        {
            base.ToggleLights(enabled);

            if (enabled && Car.IsTurnedOn)
                Lamp.material.EnableKeyword("_EMISSION");
            else Lamp.material.DisableKeyword("_EMISSION");
        }

        IEnumerator Animation(bool enabled) =>
            Animator.Animate(
                () => Animator.MoveToLocal(Lamp.transform, new Vector3(0, .007f, 0), .2f, Easing.Linear),
                () => Animator.Action(() => ToggleLights(enabled)),
                () => Animator.Wait(.05f),
                () => Animator.MoveToLocal(Lamp.transform, Vector3.zero, .2f, Easing.Linear));
        protected override IEnumerator ToggleAnimation() => Animation(true);
        protected override IEnumerator UntoggleAnimation() => Animation(false);
    }
}