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
        public static IEnumerator Animate<TVal>(IEnumerator<TVal> anim, Action<TVal> setFunc)
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




        public static TransformAnimation<T> Transform<T>(Func<float, float> easingFunc, IPercentageGetter percentageGetter,
            IEasingValuesGetter<T> valuesGetter) =>
            new TransformAnimation<T>(easingFunc, percentageGetter, valuesGetter);
        public static TransformAnimation<T> Transform<T>(Func<T, T, float, T> lerpFunc,
            Func<float, float> easingFunc, IPercentageGetter percentageGetter, IEasingValuesGetter<T> valuesGetter) =>
            new TransformAnimation<T>(lerpFunc, easingFunc, percentageGetter, valuesGetter);
    }

    public interface IPercentageGetter
    {
        float GetPercentage();
        void Restart();

        ReversePercentageGetter Reverse();
    }
    public abstract class PercentageGetter
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
    public interface IEasingValuesGetter<TVal> : IValuesGetter<TVal>
    {
        Func<TVal, TVal, float, TVal> LerpFunc { get; }
    }
    public abstract class TransformValuesGetter<TVal> : IEasingValuesGetter<TVal>
    {
        protected readonly Transform Transform;
        public abstract Func<TVal, TVal, float, TVal> LerpFunc { get; }

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
        public override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.LerpUnclamped;

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
    public class TransformPartPositionGetter : TransformValuesGetter<NullableVector3>
    {
        readonly bool Local;
        readonly NullableVector3 StartPosition, EndPosition;
        public override Func<NullableVector3, NullableVector3, float, NullableVector3> LerpFunc => NullableVector3.LerpUnclamped;

        public TransformPartPositionGetter(Transform transform, NullableVector3 startPos, NullableVector3 endPos, bool local) : base(transform)
        {
            EndPosition = endPos;
            Local = local;
            StartPosition = startPos;
        }
        public TransformPartPositionGetter(Transform transform, NullableVector3 endPos, bool local)
            : this(transform, local ? transform.localPosition : transform.position, endPos, local) { }

        public override NullableVector3 GetStartValue() => StartPosition;
        public override NullableVector3 GetEndValue() => EndPosition;

        public override void ApplyValue(NullableVector3 value)
        {
            if (Local) Transform.localPosition = value.Replace(Transform.localPosition);
            else Transform.position = value.Replace(Transform.position);
        }
    }
    public readonly struct NullableVector3
    {
        public readonly float? X, Y, Z;

        public NullableVector3(float? x = null, float? y = null, float? z = null) =>
            (X, Y, Z) = (x, y, z);

        public Vector3 Replace(Vector3 value) => new Vector3(X ?? value.x, Y ?? value.y, Z ?? value.z);

        public static NullableVector3 LerpUnclamped(NullableVector3 start, NullableVector3 end, float t)
        {
            float? x, y, z;

            if (start.X is null || end.X is null) x = null;
            else x = Mathf.LerpUnclamped(start.X.Value, end.X.Value, t);

            if (start.Y is null || end.Y is null) y = null;
            else y = Mathf.LerpUnclamped(start.Y.Value, end.Y.Value, t);

            if (start.Z is null || end.Z is null) z = null;
            else z = Mathf.LerpUnclamped(start.Z.Value, end.Z.Value, t);

            return new NullableVector3(x, y, z);
        }

        public static implicit operator NullableVector3(Vector3 vec) => new NullableVector3(vec.x, vec.y, vec.z);
    }

    public class TransformRotationGetter : TransformValuesGetter<Quaternion>
    {
        readonly bool Local;
        readonly Quaternion StartRotation, EndRotation;
        public override Func<Quaternion, Quaternion, float, Quaternion> LerpFunc => Quaternion.LerpUnclamped;

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
        public override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.LerpUnclamped;

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
        IEnumerator ReverseCoroutine();
    }
    public class TransformAnimation<TVal> : IReversableTransformAnimation
    {
        readonly Func<TVal, TVal, float, TVal> LerpFunc;
        readonly Func<float, float> EasingFunc;

        readonly IPercentageGetter PercentageGetter;
        readonly IValuesGetter<TVal> ValuesGetter;

        float Progress = 0f;

        public TransformAnimation(Func<float, float> easingFunc, IPercentageGetter percentageGetter,
            IEasingValuesGetter<TVal> valuesGetter)
        {
            LerpFunc = valuesGetter.LerpFunc;
            EasingFunc = easingFunc;
            PercentageGetter = percentageGetter;
            ValuesGetter = valuesGetter;
        }
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
            var startProgress = Mathf.Clamp(Progress, 0, 1);
            Progress = 0;

            while (true)
            {
                Progress = startProgress + PercentageGetter.GetPercentage();

                if (Progress > 1f || Progress < 0f) break;
                yield return GetValueForTime(ValuesGetter.GetStartValue(), ValuesGetter.GetEndValue(), Progress);
            }

            yield return ValuesGetter.GetEndValue();
        }

        public IEnumerator ReverseCoroutine() => Animator.Animate(ReverseValueAnimation(), ValuesGetter.ApplyValue);
        IEnumerator<TVal> ReverseValueAnimation()
        {
            PercentageGetter.Restart();
            var startProgress = Mathf.Clamp(Progress, 0, 1);
            Progress = 0;

            while (true)
            {
                Progress = startProgress - PercentageGetter.GetPercentage();

                if (Progress > 1f || Progress < 0f) break;
                yield return GetValueForTime(ValuesGetter.GetStartValue(), ValuesGetter.GetEndValue(), Progress);
            }

            yield return ValuesGetter.GetStartValue();
        }

        public void Reset() => ValuesGetter.Reset();

        protected TVal GetValueForTime(TVal start, TVal end, float percent) => LerpFunc(start, end, EasingFunc(percent));
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
        public IEnumerator ReverseCoroutine()
        {
            foreach (var animation in Animations.Reverse())
            {
                var coroutine =
                    animation is IReversableTransformAnimation rev
                    ? rev.ReverseCoroutine()
                    : animation.Coroutine();

                while (coroutine.MoveNext())
                    yield return null;
            }
        }

        public void Reset()
        {
            foreach (var anim in Animations.Reverse()) // TODO why is Reverse() here?
                anim.Reset();
        }
    }
    public class AnimationConcurrent : IReversableTransformAnimation
    {
        readonly IReadOnlyCollection<ITransformAnimation> Animations;

        public AnimationConcurrent(IEnumerable<ITransformAnimation> animations) => Animations = animations.ToArray();
        public AnimationConcurrent(params ITransformAnimation[] animations) : this(animations.AsEnumerable()) { }

        public IEnumerator Coroutine()
        {
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
        public IEnumerator ReverseCoroutine()
        {
            var enumerators = Animations
                .Reverse()
                .Select(x =>
                    x is IReversableTransformAnimation rev
                    ? rev.ReverseCoroutine()
                    : x.Coroutine())
                .ToArray();

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
    }

    public class Wait : IReversableTransformAnimation
    {
        readonly float WaitTime;

        public Wait(float seconds) => WaitTime = seconds;

        public IEnumerator Coroutine()
        {
            var time = Time.time + WaitTime;

            while (time > Time.time)
                yield return null;
        }
        public IEnumerator ReverseCoroutine() => Coroutine();

        public void Reset() { }
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