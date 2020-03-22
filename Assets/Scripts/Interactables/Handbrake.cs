using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class Handbrake : Lever
    {
        const float HandbrakeAngle = 80; // угол поворота ручника

        Quaternion StartRotation, ToRotation;
        [SerializeField] Car Car = null;

        protected override void Start()
        {
            base.Start();

            StartRotation = transform.localRotation;
            ToRotation = Quaternion.Euler(-HandbrakeAngle, 0, 0);
        }

        protected override void OnToggle()
        {
            Car.IsHandbrakeOn = true;
            StartCoroutine(Animator.RotateTo(transform, ToRotation, .7f, Easing.OutQuad));
        }
        protected override void OnUntoggle()
        {
            Car.IsHandbrakeOn = false;
            StartCoroutine(Animator.RotateTo(transform, StartRotation, .7f, Easing.OutQuad));
        }
    }
}