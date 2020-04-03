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
        public IReadOnlyInventory Inventory => _Inventory;
        readonly Inventory _Inventory = new Inventory(2);

        Vector3 CarEnterLocalPos;
        Interactable HighlightingInteractable;

        void Start()
        {
            Camera = Camera.main;

            _Inventory.OnChange += DisplayItems;
            PlayerMove = gameObject.AddComponent<PlayerMove>();
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;

            var scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scroll != 0 && _Inventory.CurrentItem != null)
            {
                _Inventory.Shift(scroll > 0 ? 1 : -1);
                DisplayItems();
            }

            var raycast = Raycast.RaycastInteractable(Camera.transform, out var hit);
            var iscar = raycast && hit.collider.gameObject.layer == LayerMask.NameToLayer("car");
            var interacted = raycast && !iscar && TryInteract(hit);

            if ((!raycast || iscar) && HighlightingInteractable != null)
            {
                HighlightingInteractable.StopHighlighting();
                HighlightingInteractable = null;
            }

            if (!interacted && _Inventory.CurrentItem != null)
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

            if (HighlightingInteractable != interactable && interactable != _Inventory.CurrentItem)
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

        public bool Pickup(Item item)
        {
            if (!_Inventory.TryAdd(item)) return false;

            item.Rigidbody.isKinematic = true;
            item.Collider.isTrigger = true;
            item.gameObject.layer = LayerMask.NameToLayer("Default");

            return true;
        }
        public void ThrowCurrentItem()
        {
            if (_Inventory.CurrentItem is null) return;

            var item = _Inventory.CurrentItem;
            RemoveItem(item);
            item.Rigidbody.AddForce(Camera.transform.forward * ThrowItemForce, ForceMode.Impulse);
        }
        public void DropCurrentItem()
        {
            if (_Inventory.CurrentItem is null) return;

            RemoveItem(_Inventory.CurrentItem);
        }
        void RemoveItem(Item item)
        {
            item.transform.SetParent(null);
            item.Rigidbody.isKinematic = false;
            item.Collider.isTrigger = false;
            item.gameObject.layer = LayerMask.NameToLayer("interactable");

            _Inventory.Remove(item);
        }

        void DisplayItems()
        {
            const float timeToPickup = .3f;

            int index = 0;
            foreach (var item in _Inventory)
            {
                if (item is null) continue;

                var pos = new Vector3(index-- / 2f, 0, 0);

                StartCoroutine(Animator.AnimateConcurrent(
                    new[]
                    {
                        Animator.MoveToWorld(item.transform, () => ItemPositionTransform.TransformPoint(pos), timeToPickup, Easing.OutQuad),
                        Animator.RotateToWorld(item.transform, () => ItemPositionTransform.rotation * item.PlayerRotation, timeToPickup, Easing.OutQuad)
                    },
                    () =>
                    {
                        if (!_Inventory.Contains(item)) return;

                        item.transform.SetParent(ItemPositionTransform);
                        item.transform.localPosition = pos;
                        item.transform.localRotation = item.PlayerRotation;
                    }));
            }
        }

        public void UseCurrentItem() => _Inventory.CurrentItem?.Use(this);
    }
}