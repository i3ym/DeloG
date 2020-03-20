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
            var rigidbody = transform.GetComponent<Rigidbody>();

            while (Time.time < endt)
            {
                yield return null;

                var rot = Quaternion.Lerp(start, end, easing((Time.time - startt) / time));

                if (rigidbody != null) rigidbody.rotation = rot;
                else transform.localRotation = rot;
            }
        }
    }
}