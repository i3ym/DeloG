using System;
using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class RotateLever : Lever
    {
        protected abstract Quaternion ToggledRotation { get; }
        protected abstract Func<float, float> ToggleEasing { get; }
        protected abstract Func<float, float> UntoggleEasing { get; }
        protected abstract float AnimTime { get; }
        Quaternion StartRotation;

        protected override void Awake()
        {
            base.Awake();
            StartRotation = transform.localRotation;
        }

        protected sealed override IEnumerator ToggleAnimation() => Animator.RotateToLocal(transform, ToggledRotation, AnimTime, ToggleEasing);
        protected sealed override IEnumerator UntoggleAnimation() => Animator.RotateToLocal(transform, StartRotation, AnimTime, UntoggleEasing);
    }
}