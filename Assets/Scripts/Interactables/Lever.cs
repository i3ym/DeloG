using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class Lever : Interactable
    {
        [SerializeField] protected Car Car = null;

        public bool IsToggled { get; private set; } = false;
        Coroutine Animation;

        public override void DoInteraction()
        {
            IsToggled = !IsToggled;

            if (Animation != null) StopCoroutine(Animation);
            Animation = StartCoroutine(IsToggled ? ToggleAnimation() : UntoggleAnimation());

            OnToggle(IsToggled);
        }

        protected abstract void OnToggle(bool toggled);

        protected abstract IEnumerator ToggleAnimation();
        protected abstract IEnumerator UntoggleAnimation();
    }
}