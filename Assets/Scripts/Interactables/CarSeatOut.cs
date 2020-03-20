using System;

namespace DeloG.Interactables
{
    public class CarSeatOut : Interactable
    {
        public event Action OnInteraction = delegate { };

        public override void DoInteraction() => OnInteraction();
    }
}