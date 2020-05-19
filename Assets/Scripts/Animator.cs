using System.Linq;
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
        public static IEnumerator Animate(IEnumerator anim1, params Func<IEnumerator>[] animations)
        {
            while (anim1.MoveNext())
                yield return null;

            var anim2 = Animate(animations);
            while (anim2.MoveNext())
                yield return null;
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
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc, Func<float> timeFunc = null) =>
            Animate(start, () => end, time, lerpFunc, easingFunc, timeFunc);

        public static IEnumerator<TVal> Animate<TVal>(TVal start, Func<TVal> end, float time,
            Func<TVal, TVal, float, TVal> lerpFunc, Func<float, float> easingFunc, Func<float> timeFunc = null)
        {
            if (timeFunc is null) timeFunc = () => Time.time;

            var startt = timeFunc();
            var endt = startt + time;

            do yield return lerpFunc(start, end(), easingFunc((timeFunc() - startt) / time));
            while (timeFunc() < endt);

            yield return end();
        }

        public static IEnumerator MoveTo(Transform transform, Vector3 end, float time, Func<float, float> easing, bool local) =>
            local ? MoveToLocal(transform, end, time, easing) : MoveToWorld(transform, end, time, easing);
        public static IEnumerator MoveTo(Transform transform, Func<Vector3> end, float time, Func<float, float> easing, bool local) =>
            local ? MoveToLocal(transform, end, time, easing) : MoveToWorld(transform, end, time, easing);

        public static IEnumerator MoveToLocal(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.LerpUnclamped, easing, (pos) => transform.localPosition = pos);
        public static IEnumerator MoveToLocal(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.LerpUnclamped, easing, (pos) => transform.localPosition = pos);

        public static IEnumerator MoveToWorld(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.LerpUnclamped, easing, (pos) => transform.position = pos);
        public static IEnumerator MoveToWorld(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.position, end, time, Vector3.LerpUnclamped, easing, (pos) => transform.position = pos);


        public static IEnumerator RotateTo(Transform transform, Quaternion end, float time, Func<float, float> easing, bool local) =>
            local ? RotateToLocal(transform, end, time, easing) : RotateToWorld(transform, end, time, easing);
        public static IEnumerator RotateTo(Transform transform, Func<Quaternion> end, float time, Func<float, float> easing, bool local) =>
            local ? RotateToLocal(transform, end, time, easing) : RotateToWorld(transform, end, time, easing);

        public static IEnumerator RotateToLocal(Transform transform, Quaternion end, float time, Func<float, float> easing) =>
            Animate(transform.localRotation, end, time, Quaternion.LerpUnclamped, easing, (rot) => transform.localRotation = rot);
        public static IEnumerator RotateToLocal(Transform transform, Func<Quaternion> end, float time, Func<float, float> easing) =>
            Animate(transform.localRotation, end, time, Quaternion.LerpUnclamped, easing, (rot) => transform.localRotation = rot);

        public static IEnumerator RotateToWorld(Transform transform, Quaternion end, float time, Func<float, float> easing) =>
            Animate(transform.rotation, end, time, Quaternion.LerpUnclamped, easing, (rot) => transform.rotation = rot);
        public static IEnumerator RotateToWorld(Transform transform, Func<Quaternion> end, float time, Func<float, float> easing) =>
            Animate(transform.rotation, end, time, Quaternion.LerpUnclamped, easing, (rot) => transform.rotation = rot);


        public static IEnumerator ScaleTo(Transform transform, Vector3 end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.LerpUnclamped, easing, (pos) => transform.localScale = pos);
        public static IEnumerator ScaleTo(Transform transform, Func<Vector3> end, float time, Func<float, float> easing) =>
            Animate(transform.localPosition, end, time, Vector3.LerpUnclamped, easing, (pos) => transform.localScale = pos);


        public interface IPercentageGetter
        {
            float GetPercentage();
            void Restart();

            ReversePercentageGetter Reverse();
        }
        public class PercentageGetter
        {
            public ReversePercentageGetter Reverse() => new ReversePercentageGetter(this as IPercentageGetter);
        }
        public class TimePercentageGetter : PercentageGetter, IPercentageGetter
        {
            readonly float Duration;
            float Start = Time.time;

            public TimePercentageGetter(float duration) => Duration = duration;

            public float GetPercentage() => (Time.time - Start) / Duration;
            public void Restart() => Start = Time.time;
        }
        public class CountPercentageGetter : PercentageGetter, IPercentageGetter
        {
            readonly int Duration;
            int Current;

            public CountPercentageGetter(int duration) => Duration = duration;

            public float GetPercentage() => (Current++) / (float) Duration;
            public void Restart() => Current = 0;
        }
        public class ReversePercentageGetter : PercentageGetter, IPercentageGetter
        {
            readonly IPercentageGetter PercentageGetter;

            public ReversePercentageGetter(IPercentageGetter getter) => PercentageGetter = getter;

            public float GetPercentage() => 1f - PercentageGetter.GetPercentage();
            public void Restart() => PercentageGetter.Restart();
        }

        public interface IValuesGetter<TVal>
        {
            TVal GetStartValue();
            TVal GetEndValue();

            void ApplyValue(TVal value);
            void Reset();
        }
        public abstract class TransformValuesGetter<TVal> : IValuesGetter<TVal>
        {
            protected readonly Transform Transform;

            public TransformValuesGetter(Transform transform) => Transform = transform;

            public abstract TVal GetStartValue();
            public abstract TVal GetEndValue();

            public abstract void ApplyValue(TVal value);

            public void Reset() => ApplyValue(GetStartValue());
        }
        public class TransformPositionGetter : TransformValuesGetter<Vector3>
        {
            readonly bool Local;
            readonly Vector3 StartPosition, EndPosition;

            public TransformPositionGetter(Transform transform, Vector3 startPos, Vector3 endPos, bool local) : base(transform)
            {
                EndPosition = endPos;
                Local = local;
                StartPosition = startPos;
            }
            public TransformPositionGetter(Transform transform, Vector3 endPos, bool local)
                : this(transform, local ? transform.localPosition : transform.position, endPos, local) { }

            public override Vector3 GetStartValue() => StartPosition;
            public override Vector3 GetEndValue() => EndPosition;

            public override void ApplyValue(Vector3 value)
            {
                if (Local) Transform.localPosition = value;
                else Transform.position = value;
            }
        }
        public class TransformRotationGetter : TransformValuesGetter<Quaternion>
        {
            readonly bool Local;
            readonly Quaternion StartRotation, EndRotation;

            public TransformRotationGetter(Transform transform, Quaternion rot, bool local) : base(transform)
            {
                EndRotation = rot;
                Local = local;
                StartRotation = Local ? Transform.localRotation : Transform.rotation;
            }

            public override Quaternion GetStartValue() => StartRotation;
            public override Quaternion GetEndValue() => EndRotation;

            public override void ApplyValue(Quaternion value)
            {
                if (Local) Transform.localRotation = value;
                else Transform.rotation = value;
            }
        }
        public class TransformScaleGetter : TransformValuesGetter<Vector3>
        {
            readonly Vector3 StartScale, EndScale;

            public TransformScaleGetter(Transform transform, Vector3 scale) : base(transform)
            {
                EndScale = scale;
                StartScale = transform.localScale;
            }

            public override Vector3 GetStartValue() => StartScale;
            public override Vector3 GetEndValue() => EndScale;

            public override void ApplyValue(Vector3 value) => Transform.localScale = value;
        }

        public interface ITransformAnimation
        {
            IEnumerator Coroutine();

            void Reset();
        }
        public interface IReversableTransformAnimation : ITransformAnimation
        {
            ITransformAnimation Reverse();
        }
        public class TransformAnimation<TVal> : IReversableTransformAnimation
        {
            readonly Func<TVal, TVal, float, TVal> LerpFunc;
            readonly Func<float, float> EasingFunc;

            readonly IPercentageGetter PercentageGetter;
            readonly IValuesGetter<TVal> ValuesGetter;

            public TransformAnimation(Func<TVal, TVal, float, TVal> lerpFunc,
                Func<float, float> easingFunc, IPercentageGetter percentageGetter, IValuesGetter<TVal> valuesGetter)
            {
                LerpFunc = lerpFunc;
                EasingFunc = easingFunc;
                PercentageGetter = percentageGetter;
                ValuesGetter = valuesGetter;
            }

            public IEnumerator Coroutine() => Animator.Animate(ValueAnimation(), ValuesGetter.ApplyValue);
            IEnumerator<TVal> ValueAnimation()
            {
                PercentageGetter.Restart();

                while (true)
                {
                    var percent = PercentageGetter.GetPercentage();
                    if (percent > 1f || percent < 0f) break;

                    yield return GetValueForTime(ValuesGetter.GetStartValue(), ValuesGetter.GetEndValue(), percent);
                }
            }

            public void Reset() => ValuesGetter.Reset();

            protected TVal GetValueForTime(TVal start, TVal end, float percent) => LerpFunc(start, end, EasingFunc(percent));

            public ITransformAnimation Reverse() =>
                new TransformAnimation<TVal>(LerpFunc, EasingFunc, PercentageGetter.Reverse(), ValuesGetter);
        }
        public class AnimationSequence : IReversableTransformAnimation
        {
            readonly IReadOnlyCollection<ITransformAnimation> Animations;

            public AnimationSequence(IEnumerable<ITransformAnimation> animations) => Animations = animations.ToArray();
            public AnimationSequence(params ITransformAnimation[] animations) : this(animations.AsEnumerable()) { }

            public IEnumerator Coroutine()
            {
                foreach (var animation in Animations)
                {
                    var coroutine = animation.Coroutine();
                    while (coroutine.MoveNext())
                        yield return null;
                }
            }

            public void Reset()
            {
                foreach (var anim in Animations.Reverse())
                    anim.Reset();
            }

            public ITransformAnimation Reverse() =>
                new AnimationSequence(Animations.Reverse().Select(x =>
                    (x is IReversableTransformAnimation rev)
                    ? rev.Reverse()
                    : x));
        }
        public class AnimationConcurrent : IReversableTransformAnimation
        {
            readonly IReadOnlyCollection<ITransformAnimation> Animations;

            public AnimationConcurrent(IEnumerable<ITransformAnimation> animations) => Animations = animations.ToArray();
            public AnimationConcurrent(params ITransformAnimation[] animations) : this(animations.AsEnumerable()) { }

            public IEnumerator Coroutine()
            {
                Reset();

                var enumerators = Animations.Select(x => x.Coroutine()).ToArray();

                bool exit = false;

                while (true)
                {
                    exit = true;
                    foreach (var animation in enumerators)
                        exit &= !animation.MoveNext();

                    if (exit) break;
                    yield return null;
                }
            }

            public void Reset()
            {
                foreach (var anim in Animations)
                    anim.Reset();
            }

            public ITransformAnimation Reverse() =>
                new AnimationConcurrent(Animations.Reverse().Select(x =>
                    (x is IReversableTransformAnimation rev)
                    ? rev.Reverse()
                    : x));
        }
    }

    public static class Easing
    {
        public static float InBack(float t) => UnityEngine.UIElements.Experimental.Easing.InBack(t);
        public static float InOutBack(float t) => UnityEngine.UIElements.Experimental.Easing.InOutBack(t);
        public static float OutBack(float t) => UnityEngine.UIElements.Experimental.Easing.OutBack(t);

        public static float InCubic(float t) => UnityEngine.UIElements.Experimental.Easing.InCubic(t);
        public static float InOutCubic(float t) => UnityEngine.UIElements.Experimental.Easing.InOutCubic(t);
        public static float OutCubic(float t) => UnityEngine.UIElements.Experimental.Easing.OutCubic(t);

        public static float InQuad(float t) => UnityEngine.UIElements.Experimental.Easing.InQuad(t);
        public static float InOutQuad(float t) => UnityEngine.UIElements.Experimental.Easing.InOutQuad(t);
        public static float OutQuad(float t) => UnityEngine.UIElements.Experimental.Easing.OutQuad(t);

        public static float Linear(float t) => UnityEngine.UIElements.Experimental.Easing.Linear(t);
    }
}