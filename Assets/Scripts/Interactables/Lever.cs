namespace DeloG.Interactables
{
    public abstract class Lever : Interactable
    {
        public bool IsToggled { get; private set; } = false;

        public override void DoInteraction()
        {
            IsToggled = !IsToggled;

            if (IsToggled) OnToggle();
            else OnUntoggle();
        }

        protected abstract void OnToggle();
        protected abstract void OnUntoggle();
    }
}