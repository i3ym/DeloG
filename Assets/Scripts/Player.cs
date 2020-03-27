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

        public Camera Camera { get; private set; }
        public PlayerMove PlayerMove { get; private set; }
        public Collider Collider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        Vector3 CarEnterLocalPos;
        Interactable HighlightingInteractable;

        public Item CurrentItem { get; private set; }

        void Start()
        {
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

            var raycast = Raycast.RaycastInteractable(Camera.transform, out var hit);
            var iscar = raycast && hit.collider.gameObject.layer == LayerMask.NameToLayer("car");
            var interacted = raycast && !iscar && TryInteract(hit);

            if ((!raycast || iscar) && HighlightingInteractable != null)
            {
                HighlightingInteractable.StopHighlighting();
                HighlightingInteractable = null;
            }

            if (!interacted && CurrentItem != null)
            {
                if (Input.GetMouseButtonDown(0)) UseCurrentItem();
                else if (Input.GetMouseButtonDown(1)) ThrowCurrentItem();
                else if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();
            }
        }

        bool TryInteract(RaycastHit hit)
        {
            var interactable = hit.collider.GetComponent<Interactable>();
            if (interactable is null) return false;

            if (HighlightingInteractable != interactable && interactable != CurrentItem)
            {
                if (HighlightingInteractable != null)
                    HighlightingInteractable.StopHighlighting();

                HighlightingInteractable = interactable;
                interactable.StartHighlighting();
            }

            if (Input.GetMouseButtonDown(0))
            {
                interactable.DoInteraction(this);
                return true;
            }

            return false;
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
            if (CurrentItem != null) DropCurrentItem();

            CurrentItem = item;
            item.Rigidbody.isKinematic = true;
            item.Collider.isTrigger = true;
            CurrentItem.gameObject.layer = LayerMask.NameToLayer("Default");

            const float timeToPickup = .3f;

            StartCoroutine(Animator.AnimateConcurrent(
                new[]
                {
                    Animator.MoveToWorld(item.transform, () => ItemPositionTransform.position, timeToPickup, Easing.OutQuad),
                    Animator.RotateToLocal(item.transform, () => ItemPositionTransform.rotation * item.PlayerRotation, timeToPickup, Easing.OutQuad)
                },
                () =>
                {
                    item.transform.SetParent(ItemPositionTransform);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = item.PlayerRotation;
                }));
        }
        public void ThrowCurrentItem()
        {
            if (CurrentItem is null) return;

            CurrentItem.transform.SetParent(null);
            CurrentItem.Rigidbody.isKinematic = false;
            CurrentItem.Collider.isTrigger = false;
            CurrentItem.gameObject.layer = LayerMask.NameToLayer("interactable");
            CurrentItem.Rigidbody.AddForce(Camera.transform.forward * ThrowItemForce, ForceMode.Impulse);

            CurrentItem = null;
        }
        public void DropCurrentItem()
        {
            if (CurrentItem is null) return;

            CurrentItem.transform.SetParent(null);
            CurrentItem.Rigidbody.isKinematic = false;
            CurrentItem.Collider.isTrigger = false;
            CurrentItem.gameObject.layer = LayerMask.NameToLayer("interactable");
        }
        public void UseCurrentItem() => CurrentItem?.Use(this);
    }
}