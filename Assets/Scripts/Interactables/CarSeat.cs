using UnityEngine;

namespace DeloG.Interactables
{
    public class CarSeat : Interactable
    {
        [SerializeField] Player Player = null;
        [SerializeField] Car Car = null;
        [SerializeField] CarSeatOut ExitCarObject = null;

        protected override void Start()
        {
            base.Start();

            ExitCarObject.OnInteraction += () =>
            {
                Player.ExitCar(Car);
                ExitCarObject.gameObject.SetActive(false);
            };
            ExitCarObject.gameObject.SetActive(false);
        }

        public override void DoInteraction()
        {
            Player.EnterCar(Car);
            ExitCarObject.gameObject.SetActive(true);
        }
    }
}