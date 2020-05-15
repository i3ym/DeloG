using UnityEngine;
using DeloG.Interactables;

namespace DeloG
{
    [RequireComponent(typeof(PlayerMove), typeof(Collider))]
    [RequireComponent(typeof(Rigidbody), typeof(ItemHolder))]
    public class Player : MonoBehaviour
    {
        const float ThrowItemForce = 20; // сила бросания предмета

        public Camera Camera { get; private set; }
        public PlayerMove PlayerMove { get; private set; }
        public Collider Collider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public ItemHolder Inventory { get; private set; }

        Interactable HighlightingInteractable;

        void Start()
        {
            Camera = Camera.main;

            PlayerMove = gameObject.AddComponent<PlayerMove>();
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();
            Inventory = GetComponent<ItemHolder>();

            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;

            var scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scroll != 0 && Inventory.CurrentItem != null)
                Inventory.Shift(scroll > 0 ? 1 : -1);

            var raycast = Raycast.RaycastInteractable(Camera.transform, out var hit);
            var iscar = raycast && hit.collider.gameObject.layer == LayerMask.NameToLayer("car");
            var interacted = raycast && !iscar && TryInteract(hit);

            if ((!raycast || iscar) && HighlightingInteractable != null)
            {
                HighlightingInteractable.StopHighlighting();
                HighlightingInteractable = null;
            }

            if (!interacted && Inventory.CurrentItem != null)
            {
                if (Input.GetMouseButtonDown(0)) UseCurrentItem();
                else if (Input.GetMouseButtonDown(1)) ThrowCurrentItem();
                else if (Input.GetKeyDown(KeyCode.Q)) DropCurrentItem();
            }
        }

        bool TryInteract(RaycastHit hit)
        {
            var interactable = hit.collider.GetComponent<Interactable>();
            if (interactable is null || !interactable) return false;

            if (HighlightingInteractable != interactable && interactable != Inventory.CurrentItem)
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

        public void ThrowCurrentItem()
        {
            if (Inventory.CurrentItem is null) return;

            var item = Inventory.CurrentItem;
            Inventory.RemoveItem(item);
            item.Rigidbody.AddForce(Camera.transform.forward * ThrowItemForce, ForceMode.Impulse);
        }
        public void DropCurrentItem()
        {
            if (Inventory.CurrentItem is null) return;
            Inventory.RemoveItem(Inventory.CurrentItem);
        }
        public void UseCurrentItem() => Inventory.CurrentItem?.Use(this);
    }
}