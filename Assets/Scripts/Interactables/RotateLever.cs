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

        protected override void Start()
        {
            base.Start();
            StartRotation = transform.localRotation;
        }

        protected sealed override IEnumerator ToggleAnimation() => Animator.RotateTo(transform, ToggledRotation, AnimTime, ToggleEasing);
        protected sealed override IEnumerator UntoggleAnimation() => Animator.RotateTo(transform, StartRotation, AnimTime, UntoggleEasing);
    }
}