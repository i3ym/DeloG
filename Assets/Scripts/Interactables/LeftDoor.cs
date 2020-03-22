using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class LeftDoor : Door
    {
        protected const float MaxAngle = 80; // угол открытой двери

        public override IEnumerator OpenAnimation() =>
            Animator.RotateTo(transform, Quaternion.Euler(0, MaxAngle, 0), 1f, Easing.OutCubic);
        public override IEnumerator CloseAnimation() =>
            Animator.RotateTo(transform, Quaternion.identity, 1f, Easing.OutCubic);
    }
}