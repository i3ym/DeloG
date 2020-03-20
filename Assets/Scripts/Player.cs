using UnityEngine;
using DeloG.Interactables;

namespace DeloG
{
    public class Player : MonoBehaviour
    {
        int CarLayerMask, InteractableLayerMask;
        Camera Camera;
        PlayerMove PlayerMove;
        Collider Collider;
        Rigidbody Rigidbody;

        Vector3 CarEnterLocalPos;

        void Start()
        {
            InteractableLayerMask = LayerMask.GetMask("interactable", "car");
            CarLayerMask = LayerMask.GetMask("car");
            Camera = Camera.main;

            PlayerMove = gameObject.AddComponent<PlayerMove>();
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;

            if (Input.GetMouseButtonDown(0))
            {
                var raycast = Physics.Raycast(Camera.transform.position, Camera.transform.forward, out var hit, 10f, InteractableLayerMask);
                if (raycast) hit.collider.GetComponent<Interactable>()?.DoInteraction();
            }
        }

        public void EnterCar(Car car)
        {
            PlayerMove.enabled = false;
            Collider.enabled = false;
            car.enabled = true;
            transform.SetParent(car.transform);
            CarEnterLocalPos = transform.localPosition;
            transform.localPosition = car.PlayerPosition.localPosition;
            transform.localRotation = default;
            Rigidbody.isKinematic = true;
        }
        public void ExitCar(Car car)
        {
            PlayerMove.enabled = true;
            Collider.enabled = true;
            car.enabled = false;
            transform.localPosition = CarEnterLocalPos;
            transform.SetParent(null);
            Rigidbody.isKinematic = false;
        }
    }
}