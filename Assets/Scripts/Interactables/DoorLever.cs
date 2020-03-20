using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class DoorLever : Lever
    {
        [SerializeField] Door Door = null;

        protected override void OnToggle() => StartCoroutine(Door.OpenAnimation());
        protected override void OnUntoggle() => StartCoroutine(Door.CloseAnimation());
    }
}