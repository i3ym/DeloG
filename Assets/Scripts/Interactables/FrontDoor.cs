using System;
using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class FrontDoor : Door
    {
        [SerializeField] Side DoorSide;
        protected const float MaxAngle = 80; // угол открытой двери

        IReversableTransformAnimation Animation;

        void Awake()
        {
            var maxAngle =
                DoorSide == Side.Left ? MaxAngle
                : DoorSide == Side.Right ? -MaxAngle
                : throw new ArgumentException(nameof(DoorSide) + " is invalid", nameof(DoorSide));

            Animation = new TransformAnimation<Quaternion>(
                Easing.OutCubic,
                new TimePercentageGetter(1f),
                new TransformRotationGetter(transform, Quaternion.Euler(0, maxAngle, 0), true));
        }

        public override IEnumerator OpenAnimation() => Animation.Coroutine();
        public override IEnumerator CloseAnimation() => Animation.ReverseCoroutine();


        enum Side : byte { Left, Right }
    }
}