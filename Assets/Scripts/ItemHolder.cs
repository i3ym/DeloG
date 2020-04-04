using System.Collections;
using System.Collections.Generic;
using DeloG.Items;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG
{
    public class ItemHolder : MonoBehaviour, IReadOnlyInventory
    {
        public Vector3 LocalTargerPosition => ItemPositionTransform.localPosition;
        public Quaternion LocalTargerRotation => ItemPositionTransform.localRotation;

        public Item CurrentItem => Inventory.CurrentItem;
        public int Count => Inventory.Count;
        public int Capacity => Inventory.Capacity;

        [SerializeField] Transform ItemPositionTransform = null;
        [SerializeField] int InventoryCapacity = 2;

        Inventory Inventory;

        void Awake()
        {
            Inventory = new Inventory(InventoryCapacity);
            Inventory.OnChange += DisplayItems;
        }

        public bool Pickup(Item item)
        {
            if (!Inventory.TryAdd(item)) return false;

            item.Rigidbody.isKinematic = true;
            item.Collider.isTrigger = true;
            item.gameObject.layer = LayerMask.NameToLayer("Default");

            return true;
        }
        public void RemoveItem(Item item)
        {
            if (!Inventory.Contains(item)) return;

            item.transform.SetParent(null);
            item.Rigidbody.isKinematic = false;
            item.Collider.isTrigger = false;
            item.gameObject.layer = LayerMask.NameToLayer("interactable");

            Inventory.Remove(item);
        }
        public void Shift(int amount) => Inventory.Shift(amount);

        void DisplayItems()
        {
            const float timeToPickup = .3f;

            int index = 0;
            foreach (var item in Inventory)
            {
                if (item is null) continue;

                var pos = new Vector3(index-- / 2f, 0, 0);

                StartCoroutine(Animator.AnimateConcurrent(
                    new[]
                    {
                        Animator.MoveToWorld(item.transform, () => ItemPositionTransform.TransformPoint(pos), timeToPickup, Easing.OutQuad),
                        Animator.RotateToWorld(item.transform, () => ItemPositionTransform.rotation * item.PlayerRotation, timeToPickup, Easing.OutQuad),
                    },
                    () =>
                    {
                        if (!Inventory.Contains(item)) return;

                        item.transform.SetParent(ItemPositionTransform);
                        item.transform.localPosition = pos;
                        item.transform.localRotation = item.PlayerRotation;
                    }));
            }
        }

        public IEnumerator<Item> GetEnumerator() => Inventory.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}