using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class Lever : CarInteractable
    {
        public bool IsToggled { get; private set; } = false;
        Coroutine Animation;

        public override void DoInteraction(Player player) => Toggle(!IsToggled);
        protected void Toggle(bool toggle)
        {
            IsToggled = toggle;

            if (Animation != null) StopCoroutine(Animation);

            var anim = IsToggled ? ToggleAnimation() : UntoggleAnimation();
            if (anim != null) Animation = StartCoroutine(anim);

            OnToggle(IsToggled);
        }

        protected abstract void OnToggle(bool toggled);

        protected abstract IEnumerator ToggleAnimation();
        protected abstract IEnumerator UntoggleAnimation();
    }
}