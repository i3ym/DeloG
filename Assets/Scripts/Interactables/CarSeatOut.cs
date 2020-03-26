using System;

namespace DeloG.Interactables
{
    public class CarSeatOut : Interactable
    {
        public event Action<Player> OnInteraction = delegate { };

        public override void DoInteraction(Player player) => OnInteraction(player);
    }
}