using System.Collections;
using UnityEngine;

namespace DeloG
{
    public class EasingTest : MonoBehaviour
    {
        IEnumerator CotourineEnumerator;
        Coroutine Coroutine;
        IReversableTransformAnimation Sequence;

        void Awake()
        {
            Sequence =
                new AnimationSequence(
                    new TransformAnimation<Vector3>(
                        Easing.Linear,
                        new TimePercentageGetter(1f),
                        new TransformPositionGetter(transform, Vector2.one, false)),

                    new TransformAnimation<Vector3>(
                        Easing.Linear,
                        new TimePercentageGetter(1f),
                        new TransformScaleGetter(transform, Vector3.one))
                );
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