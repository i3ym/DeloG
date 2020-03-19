using UnityEngine;

namespace DeloG
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Car Car = null;

        int CarLayerMask;
        Camera Camera;
        PlayerMove PlayerMove;
        Collider Collider;
        Rigidbody Rigidbody;

        void Start()
        {
            CarLayerMask = LayerMask.GetMask("car");
            Camera = Camera.main;

            PlayerMove = gameObject.AddComponent<PlayerMove>();
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;

            Car.enabled = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;

            TryEnterCar();
        }

        void TryEnterCar()
        {
            if (!Input.GetKeyDown(KeyCode.F)) return;

            if (PlayerMove.enabled)
            {
                var raycast = Physics.Raycast(Camera.transform.position, Camera.transform.forward, 5, CarLayerMask);
                if (!raycast) return;

                PlayerMove.enabled = false;
                Collider.enabled = false;
                Car.enabled = true;
                transform.SetParent(Car.transform);
                transform.localPosition = Car.PlayerPosition.localPosition;
                transform.localRotation = default;
                Rigidbody.isKinematic = true;
            }
            else
            {
                PlayerMove.enabled = true;
                Collider.enabled = true;
                Car.enabled = false;
                transform.SetParent(null);
                transform.position = Car.transform.position;
                Rigidbody.isKinematic = false;
            }
        }
    }
}