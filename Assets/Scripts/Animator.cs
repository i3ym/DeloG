using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public static class Animator
    {
        public static IEnumerator Animate<TVal>(TVal start, TVal end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc, Action<TVal> setFunc)
        {
            var anim = Animate(start, end, time, lerpFunc, easingFunc);

            while (anim.MoveNext())
            {
                yield return null;
                setFunc(anim.Current);
            }
        }
        public static IEnumerator Animate<TVal>(TVal start, Func<TVal> end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc, Action<TVal> setFunc)
        {
            var anim = Animate(start, end, time, lerpFunc, easingFunc);

            while (anim.MoveNext())
            {
                yield return null;
                setFunc(anim.Current);
            }
        }

        public static IEnumerator<TVal> Animate<TVal>(TVal start, TVal end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc)
        {
            var startt = Time.time;
            var endt = startt + time;

            do yield return lerpFunc(start, end, easingFunc((Time.time - startt) / time));
            while (Time.time < endt);
        }
        public static IEnumerator<TVal> Animate<TVal>(TVal start, Func<TVal> end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc)
        {
            var startt = Time.time;
            var endt = startt + time;

            do yield return lerpFunc(start, end(), easingFunc((Time.time - startt) / time));
            while (Time.time < endt);
        }

        public static IEnumerator MoveToLocal(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (rot) => transform.localPosition = rot);
        public static IEnumerator MoveToLocal(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (rot) => transform.localPosition = rot);
        public static IEnumerator MoveToWorld(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.Lerp, easing, (rot) => transform.position = rot);
        public static IEnumerator MoveToWorld(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.Lerp, easing, (rot) => transform.position = rot);

        public static IEnumerator RotateToLocal(Transform transform, Quaternion end, float time, Func<float, float> easing) =>
            Animate(transform.localRotation, end, time, Quaternion.Lerp, easing, (rot) => transform.localRotation = rot);
        public static IEnumerator RotateToWorld(Transform transform, Quaternion end, float time, Func<float, float> easing) =>
            Animate(transform.rotation, end, time, Quaternion.Lerp, easing, (rot) => transform.rotation = rot);
    }
}