using UnityEngine;

namespace DeloG
{
    public class EasingTest : MonoBehaviour
    {
        Coroutine Coroutine;

        Animator.ITransformAnimation Sequence;

        void Awake()
        {
            var seq = new Animator.AnimationSequence(
                new Animator.TransformAnimation<Vector3>(
                    Vector3.LerpUnclamped, Easing.InBack,
                    () => new Animator.TimePercentageGetter(1f),
                    () => new Animator.TransformPositionGetter(transform, Vector2.one, false)),
                new Animator.TransformAnimation<Vector3>(
                    Vector3.LerpUnclamped, Easing.InBack,
                    () => new Animator.TimePercentageGetter(1f),
                    () => new Animator.TransformScaleGetter(transform, Vector3.one)));

            Sequence = new Animator.AnimationSequence(seq, seq.Reverse());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Coroutine != null) StopCoroutine(Coroutine);

                transform.position = Vector3.zero;
                Coroutine = StartCoroutine(Sequence.Coroutine());
            }

            if (false && Input.GetKeyDown(KeyCode.Space))
            {
                if (Coroutine != null) StopCoroutine(Coroutine);

                transform.position = Vector3.zero;
                Coroutine = StartCoroutine(
                    Animator.AnimateConcurrent(
                        Animator.Animate(0f, 1f, 1f, Mathf.LerpUnclamped,
                            Easing.Linear, (x) => transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z)),
                        Animator.Animate(0f, 1f, 1f, Mathf.LerpUnclamped,
                            Method, (y) => transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z))
                    ));
            }
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