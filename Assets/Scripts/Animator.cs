using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public static class Animator
    {
        public static IEnumerator Animate(IEnumerable<IEnumerator> animations, Action after = null)
        {
            foreach (var animation in animations)
                while (animation.MoveNext())
                    yield return null;

            after?.Invoke();
        }
        public static IEnumerator AnimateConcurrent(IEnumerable<IEnumerator> animations, Action after = null)
        {
            bool exit = false;

            while (true)
            {
                exit = true;
                foreach (var animation in animations)
                    exit = exit & !animation.MoveNext();

                if (exit) break;
                yield return null;
            }

            after?.Invoke();
        }

        public static IEnumerator Animate<TVal>(TVal start, TVal end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc, Action<TVal> setFunc) =>
            Animate(Animate(start, end, time, lerpFunc, easingFunc), setFunc);
        public static IEnumerator Animate<TVal>(TVal start, Func<TVal> end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc, Action<TVal> setFunc) =>
            Animate(Animate(start, end, time, lerpFunc, easingFunc), setFunc);
        static IEnumerator Animate<TVal>(IEnumerator<TVal> anim, Action<TVal> setFunc)
        {
            while (anim.MoveNext())
            {
                setFunc(anim.Current);
                yield return null;
            }
        }

        public static IEnumerator<TVal> Animate<TVal>(TVal start, TVal end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc)
        {
            var startt = Time.time;
            var endt = startt + time;

            do yield return lerpFunc(start, end, easingFunc((Time.time - startt) / time));
            while (Time.time < endt);

            yield return end;
        }
        public static IEnumerator<TVal> Animate<TVal>(TVal start, Func<TVal> end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc)
        {
            var startt = Time.time;
            var endt = startt + time;

            do yield return lerpFunc(start, end(), easingFunc((Time.time - startt) / time));
            while (Time.time < endt);

            yield return end();
        }

        public static IEnumerator MoveTo(Transform transform, Vector3 end, float time, Func<float, float> easing, bool local) =>
            local ? MoveToLocal(transform, end, time, easing) : MoveToWorld(transform, end, time, easing);
        public static IEnumerator MoveTo(Transform transform, Func<Vector3> end, float time, Func<float, float> easing, bool local) =>
            local ? MoveToLocal(transform, end, time, easing) : MoveToWorld(transform, end, time, easing);

        public static IEnumerator MoveToLocal(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (rot) => transform.localPosition = rot);
        public static IEnumerator MoveToLocal(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (rot) => transform.localPosition = rot);

        public static IEnumerator MoveToWorld(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.Lerp, easing, (rot) => transform.position = rot);
        public static IEnumerator MoveToWorld(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.Lerp, easing, (rot) => transform.position = rot);


        public static IEnumerator RotateTo(Transform transform, Quaternion end, float time, Func<float, float> easing, bool local) =>
            local ? RotateToLocal(transform, end, time, easing) : RotateToWorld(transform, end, time, easing);
        public static IEnumerator RotateTo(Transform transform, Func<Quaternion> end, float time, Func<float, float> easing, bool local) =>
            local ? RotateToLocal(transform, end, time, easing) : RotateToWorld(transform, end, time, easing);

        public static IEnumerator RotateToLocal(Transform transform, Quaternion end, float time, Func<float, float> easing) =>
            Animate(transform.localRotation, end, time, Quaternion.Lerp, easing, (rot) => transform.localRotation = rot);
        public static IEnumerator RotateToLocal(Transform transform, Func<Quaternion> end, float time, Func<float, float> easing) =>
            Animate(transform.localRotation, end, time, Quaternion.Lerp, easing, (rot) => transform.localRotation = rot);

        public static IEnumerator RotateToWorld(Transform transform, Quaternion end, float time, Func<float, float> easing) =>
            Animate(transform.rotation, end, time, Quaternion.Lerp, easing, (rot) => transform.rotation = rot);
        public static IEnumerator RotateToWorld(Transform transform, Func<Quaternion> end, float time, Func<float, float> easing) =>
            Animate(transform.rotation, end, time, Quaternion.Lerp, easing, (rot) => transform.rotation = rot);
    }
}