using UnityEngine;

namespace DeloG.Interactables
{
    public class DoorLever : Lever
    {
        const float MaxAngle = 80; // угол открытой двери

        [SerializeField] Transform Door = null;

        protected override void OnToggle()
        {
            Door.Rotate(Door.up, MaxAngle, Space.World);
        }
        protected override void OnUntoggle()
        {
            Door.Rotate(Door.up, -MaxAngle, Space.World);
        }
    }
}