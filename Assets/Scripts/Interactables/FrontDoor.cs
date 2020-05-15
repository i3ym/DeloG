using System;
using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class FrontDoor : Door
    {
        [SerializeField] Side DoorSide;
        protected const float MaxAngle = 80; // угол открытой двери

        public override IEnumerator OpenAnimation()
        {
            var maxAngle =
                DoorSide == Side.Left ? MaxAngle
                : DoorSide == Side.Right ? -MaxAngle
                : throw new ArgumentException();

            return Animator.RotateToLocal(transform, Quaternion.Euler(0, maxAngle, 0), 1f, Easing.OutCubic);
        }
        public override IEnumerator CloseAnimation() =>
            Animator.RotateToLocal(transform, Quaternion.identity, 1f, Easing.OutCubic);


        enum Side : byte { Left, Right }
    }
}