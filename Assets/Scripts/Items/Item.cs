using DeloG.Interactables;
using UnityEngine;

namespace DeloG.Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Renderer))]
    public class Item : Interactable
    {
        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();

            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        public sealed override void DoInteraction(Player player) => player.Inventory.Pickup(this);
        public virtual void Use(Player player) { }
    }
}