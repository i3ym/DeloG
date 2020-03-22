using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class DoorLever : Lever
    {
        [SerializeField] Door Door = null;

        protected override void OnToggle(bool toggled) { }

        protected override IEnumerator ToggleAnimation() => Door.OpenAnimation();
        protected override IEnumerator UntoggleAnimation() => Door.CloseAnimation();
    }
}