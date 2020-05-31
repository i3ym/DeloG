using System.Collections;
using UnityEngine;

namespace DeloG
{
    public class EasingTest : MonoBehaviour
    {
        IEnumerator CotourineEnumerator;
        Coroutine Coroutine;
        Animator.IReversableTransformAnimation Sequence;

        void Awake()
        {
            Sequence =
                new Animator.AnimationSequence(
                    new Animator.TransformAnimation<Vector3>(
                        Vector3.LerpUnclamped, Easing.Linear,
                        new Animator.TimePercentageGetter(1f),
                        new Animator.TransformPositionGetter(transform, Vector2.one, false)),

                    new Animator.TransformAnimation<Vector3>(
                        Vector3.LerpUnclamped, Easing.Linear,
                        new Animator.TimePercentageGetter(1f),
                        new Animator.TransformScaleGetter(transform, Vector3.one))
                );

            /*var seq = new Animator.AnimationSequence(
                new Animator.TransformAnimation<Vector3>(
                    Vector3.LerpUnclamped, Easing.InBack,
                    new Animator.TimePercentageGetter(1f),
                    new Animator.TransformPositionGetter(transform, Vector2.one, false)),
                new Animator.TransformAnimation<Vector3>(
                    Vector3.LerpUnclamped, Easing.OutQuad,
                    new Animator.TimePercentageGetter(1f),
                    new Animator.TransformScaleGetter(transform, Vector3.one)));

            Sequence = new Animator.AnimationSequence(seq, seq.Reverse(), seq);*/
        }

        bool xyu = true;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Coroutine != null) StopCoroutine(Coroutine);

                if (xyu) Coroutine = StartCoroutine(Sequence.Coroutine());
                else Coroutine = StartCoroutine(Sequence.ReverseCoroutine());

                xyu = !xyu;
            }

            if (Input.GetKeyDown(KeyCode.E) && Coroutine != null)
                StopCoroutine(Coroutine);
        }


        static float Method(float t) => OpenEasing1(t);
        static float OpenEasing1(float t)
        {
            const float c1 = Mathf.PI / 2f;
            const float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        }
    }
}