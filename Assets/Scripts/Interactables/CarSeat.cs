using UnityEngine;

namespace DeloG.Interactables
{
    public class CarSeat : Interactable
    {
        [SerializeField] Car Car = null;
        [SerializeField] CarSeatOut ExitCarObject = null;

        protected override void Awake()
        {
            base.Awake();

            ExitCarObject.OnInteraction += (player) =>
            {
                player.ExitCar(Car);
                ExitCarObject.gameObject.SetActive(false);
            };
            ExitCarObject.gameObject.SetActive(false);
        }

        public override void DoInteraction(Player player)
        {
            player.EnterCar(Car);
            ExitCarObject.gameObject.SetActive(true);
        }
    }
}