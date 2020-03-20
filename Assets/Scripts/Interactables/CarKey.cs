using UnityEngine;

namespace DeloG.Interactables
{
    public class CarKey : Lever
    {
        const float KeyAngle = 90f; // угол поворота ключей зажигания

        [SerializeField] Car Car = null;

        protected override void OnToggle()
        {
            Car.IsTurnedOn = true;
            transform.Rotate(transform.forward, -KeyAngle, Space.World);
        }
        protected override void OnUntoggle()
        {
            Car.IsTurnedOn = false;
            transform.Rotate(transform.forward, KeyAngle, Space.World);
        }
    }
}