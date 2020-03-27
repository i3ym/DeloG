using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class RightDoor : LeftDoor
    {
        public override IEnumerator OpenAnimation() =>
            Animator.RotateToLocal(transform, new Quaternion(0, -MaxAngle / 90f, 0, 1), 1f, Easing.OutCubic);
        public override IEnumerator CloseAnimation() =>
            Animator.RotateToLocal(transform, Quaternion.identity, 1f, Easing.OutCubic);
    }
}