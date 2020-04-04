using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class BackDoor : Door
    {
        Vector3 StartLocalPosition;

        void Awake() => StartLocalPosition = transform.localPosition;

        public override IEnumerator OpenAnimation() =>
            Animator.Animate(
                () => Animator.MoveToLocal(transform, StartLocalPosition + new Vector3(.2f, 0, 0), .7f, Easing.Linear),
                () => Animator.MoveToLocal(transform, StartLocalPosition + new Vector3(.2f, 0, -1.7f), 1f, Easing.OutCubic));

        public override IEnumerator CloseAnimation() =>
            Animator.Animate(
                () => Animator.MoveToLocal(transform, StartLocalPosition + new Vector3(.2f, 0, 0f), 1f, Easing.OutCubic),
                () => Animator.MoveToLocal(transform, StartLocalPosition, .7f, Easing.Linear));
    }
}