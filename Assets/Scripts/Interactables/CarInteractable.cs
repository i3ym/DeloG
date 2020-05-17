using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class CarInteractable : Interactable
    {
        [SerializeField] protected Car Car = null;

        protected override void Awake()
        {
            if (Car is null)
            {
                var obj = transform;

                while (true)
                {
                    if (!obj) break;

                    var car = obj.GetComponent<Car>();
                    if (car)
                    {
                        Car = car;
                        break;
                    }

                    obj = obj.parent;
                }

                if (Car is null)
                    Debug.LogWarning("Объект машины не был найден для объекта " + name, this);
            }

            base.Awake();
        }
    }
}