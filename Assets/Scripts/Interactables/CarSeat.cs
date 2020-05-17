using UnityEngine;

namespace DeloG.Interactables
{
    public class CarSeat : CarInteractable
    {
        [SerializeField] CarSeatOut ExitCarObject = null;

        protected override void Awake()
        {
            base.Awake();

            ExitCarObject.OnInteraction += (player) =>
            {
                Car.Exit(player);
                ExitCarObject.gameObject.SetActive(false);
            };
            ExitCarObject.gameObject.SetActive(false);
        }

        public override void DoInteraction(Player player)
        {
            Car.Enter(player);
            ExitCarObject.gameObject.SetActive(true);
        }
    }
}