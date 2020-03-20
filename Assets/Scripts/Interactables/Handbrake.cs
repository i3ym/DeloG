using UnityEngine;

namespace DeloG.Interactables
{
    public class Handbrake : Lever
    {
        const float HandbrakeAngle = 30; // угол поворота ручника

        [SerializeField] Car Car = null;

        protected override void OnToggle()
        {
            Car.IsHandbrakeOn = true;
            transform.Rotate(transform.right, -HandbrakeAngle, Space.World);
        }
        protected override void OnUntoggle()
        {
            Car.IsHandbrakeOn = false;
            transform.Rotate(transform.right, HandbrakeAngle, Space.World);
        }
    }
}