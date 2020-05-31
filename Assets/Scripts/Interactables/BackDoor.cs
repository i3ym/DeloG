using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class BackDoor : Door
    {
        Vector3 StartLocalPosition;

        IReversableTransformAnimation Animation;

        void Awake()
        {
            StartLocalPosition = transform.localPosition;

            Animation = new AnimationConcurrent(
                new AnimationSequence(
                    Animator.Transform(
                        Easing.OutBack,
                        new TimePercentageGetter(.7f),
                        new TransformPartPositionGetter(
                            transform,
                            new NullableVector3(x: transform.localPosition.x - .2f),
                            true)
                    ),
                    new Wait(.3f)
                ),
                Animator.Transform(
                    Easing.Linear,
                    new TimePercentageGetter(1f),
                    new TransformPartPositionGetter(
                        transform,
                        new NullableVector3(z: transform.localPosition.z + 1.7f),
                        true)
                )
            );
        }

        public override IEnumerator OpenAnimation() => Animation.Coroutine();
        public override IEnumerator CloseAnimation() => Animation.ReverseCoroutine();
    }
}