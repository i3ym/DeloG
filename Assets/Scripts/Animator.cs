using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public static class Animator
    {
        public static IEnumerator Animate(IEnumerator animation, Action after)
        {
            while (animation.MoveNext())
                yield return animation.Current;

            after();
        }
        public static IEnumerator Animate(params Func<IEnumerator>[] animations)
        {
            foreach (var anim in animations)
            {
                var animation = anim();
                while (animation.MoveNext())
                    yield return null;
            }
        }
        public static IEnumerator AnimateConcurrent(params IEnumerator[] animations) => AnimateConcurrent(animations, null);
        public static IEnumerator AnimateConcurrent(Action after, params IEnumerator[] animations) => AnimateConcurrent(animations, after);
        public static IEnumerator AnimateConcurrent(IEnumerable<IEnumerator> animations, Action after = null)
        {
            bool exit = false;

            while (true)
            {
                exit = true;
                foreach (var animation in animations)
                    exit &= !animation.MoveNext();

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


        public static IEnumerator Action(Action action)
        {
            action();
            yield break;
        }
        public static IEnumerator Wait(float seconds)
        {
            var time = Time.time + seconds;

            while (time > Time.time)
                yield return null;
        }
        public static IEnumerator WaitForFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
                yield return null;
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
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (pos) => transform.localPosition = pos);
        public static IEnumerator MoveToLocal(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (pos) => transform.localPosition = pos);

        public static IEnumerator MoveToWorld(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.Lerp, easing, (pos) => transform.position = pos);
        public static IEnumerator MoveToWorld(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.Lerp, easing, (pos) => transform.position = pos);


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


        public static IEnumerator ScaleTo(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (pos) => transform.localScale = pos);
        public static IEnumerator ScaleTo(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.Lerp, easing, (pos) => transform.localScale = pos);
    }

    public static class Easing
    {
        public static float InOutCubic(float t) => UnityEngine.UIElements.Experimental.Easing.InOutCubic(t);
        public static float OutCubic(float t) => UnityEngine.UIElements.Experimental.Easing.OutCubic(t);

        public static float InOutQuad(float t) => UnityEngine.UIElements.Experimental.Easing.InOutQuad(t);
        public static float OutQuad(float t) => UnityEngine.UIElements.Experimental.Easing.OutQuad(t);

        public static float Linear(float t) => UnityEngine.UIElements.Experimental.Easing.Linear(t);
    }
}