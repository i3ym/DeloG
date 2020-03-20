using System;
using System.Collections;
using UnityEngine;

namespace DeloG
{
    public static class Animator
    {
        public static IEnumerator MoveTo(Transform transform, Vector3 end, float time, Func<float, float> easing)
        {
            var startt = Time.time;
            var endt = startt + time;
            var start = transform.localPosition;

            while (Time.time < endt)
            {
                yield return null;
                transform.localPosition = Vector3.Lerp(start, end, easing((Time.time - startt) / time));
            }
        }

        public static IEnumerator RotateTo(Transform transform, Quaternion end, float time,
            Func<float, float> easing)
        {
            var startt = Time.time;
            var endt = startt + time;
            var start = transform.localRotation;

            while (Time.time < endt)
            {
                yield return null;
                transform.localRotation = Quaternion.Lerp(start, end, easing((Time.time - startt) / time));
            }
        }
    }
}