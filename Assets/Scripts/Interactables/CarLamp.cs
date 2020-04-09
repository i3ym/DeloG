using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class CarLamp : CarLightsLever
    {
        [SerializeField] Transform MoveTransform = null;

        IEnumerator Animation(bool enabled) =>
            Animator.Animate(
                () => Animator.MoveToLocal(MoveTransform, new Vector3(0, .007f, 0), .2f, Easing.Linear),
                () => Animator.Action(() => ToggleLights(enabled)),
                () => Animator.Wait(.05f),
                () => Animator.MoveToLocal(MoveTransform, Vector3.zero, .2f, Easing.Linear));
        protected override IEnumerator ToggleAnimation() => Animation(true);
        protected override IEnumerator UntoggleAnimation() => Animation(false);
    }
}