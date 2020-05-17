using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class BackDoor : Door
    {
        Vector3 StartLocalPosition;

        void Awake() => StartLocalPosition = transform.localPosition;

        public override IEnumerator OpenAnimation() =>
            Animator.AnimateConcurrent(
                Animator.Animate(transform.localPosition.x, StartLocalPosition.x - .2f, .7f, Mathf.LerpUnclamped,
                    Easing.OutBack, (x) => transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z)),
                Animator.Animate(transform.localPosition.z, StartLocalPosition.z + 1.7f, 1f, Mathf.LerpUnclamped,
                    Easing.Linear, (z) => transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z))
            );

        public override IEnumerator CloseAnimation() =>
            Animator.AnimateConcurrent(
                Animator.Animate(
                    Animator.Wait(.3f),
                    () => Animator.Animate(transform.localPosition.x, StartLocalPosition.x, .7f, Mathf.LerpUnclamped,
                        Easing.InBack, (x) => transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z))),
                Animator.Animate(transform.localPosition.z, StartLocalPosition.z, 1f, Mathf.LerpUnclamped,
                    Easing.Linear, (z) => transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z))
            );
    }
}