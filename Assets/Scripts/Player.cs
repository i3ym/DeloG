using UnityEngine;
using DeloG.Interactables;
using DeloG.Items;
using UnityEngine.UIElements.Experimental;

namespace DeloG
{
    public class Player : MonoBehaviour
    {
        const float ThrowItemForce = 20; // сила бросания предмета

        [SerializeField] Transform ItemPositionTransform = null;

        int CarLayerMask, InteractableLayerMask;
        Camera Camera;
        PlayerMove PlayerMove;
        Collider Collider;
        Rigidbody Rigidbody;

        Vector3 CarEnterLocalPos;
        Interactable HighlightingInteractable;

        Item CurrentItem;

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

            var raycast = Physics.Raycast(Camera.transform.position, Camera.transform.forward, out var hit, 10f, InteractableLayerMask);

            if (raycast)
            {
                var interactable = hit.collider.GetComponent<Interactable>();

                if (HighlightingInteractable != interactable)
                {
                    if (HighlightingInteractable != null)
                        HighlightingInteractable.StopHighlighting();

                    HighlightingInteractable = interactable;
                    interactable.StartHighlighting();
                }

                if (Input.GetMouseButtonDown(0)) interactable.DoInteraction(this);
            }
            else
            {
                if (CurrentItem != null)
                {
                    if (Input.GetMouseButtonDown(0)) ThrowCurrentItem();
                    else if (Input.GetMouseButtonDown(1)) ReleaseCurrentItem();
                }

                if (HighlightingInteractable != null)
                {
                    HighlightingInteractable.StopHighlighting();
                    HighlightingInteractable = null;
                }
            }
        }

        public void EnterCar(Car car)
        {
            PlayerMove.enabled = false;
            Collider.enabled = false;
            car.Enabled = true;
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
            car.Enabled = false;
            transform.localPosition = CarEnterLocalPos;
            transform.SetParent(null);
            transform.localRotation = default;
            Rigidbody.isKinematic = false;
        }

        public void Pickup(Item item)
        {
            if (CurrentItem != null) ReleaseCurrentItem();

            CurrentItem = item;
            item.transform.SetParent(ItemPositionTransform);
            item.Rigidbody.isKinematic = true;
            item.Collider.isTrigger = true;

            const float timeToPickup = .3f;
            StartCoroutine(Animator.MoveToWorld(item.transform, () => ItemPositionTransform.position, timeToPickup, Easing.OutQuad));
            StartCoroutine(Animator.RotateToLocal(item.transform, Quaternion.identity, timeToPickup, Easing.OutQuad));
        }
        public void ThrowCurrentItem()
        {
            CurrentItem.transform.SetParent(null);
            CurrentItem.Rigidbody.isKinematic = false;
            CurrentItem.Collider.isTrigger = false;
            CurrentItem.Rigidbody.AddForce(Camera.transform.forward * ThrowItemForce, ForceMode.Impulse);

            CurrentItem = null;
        }
        public void ReleaseCurrentItem()
        {
            CurrentItem.transform.SetParent(null);
            CurrentItem.Rigidbody.isKinematic = false;
            CurrentItem.Collider.isTrigger = false;
        }
    }
}