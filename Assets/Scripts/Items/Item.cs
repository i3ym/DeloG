using DeloG.Interactables;
using UnityEngine;

namespace DeloG.Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Renderer))]
    public class Item : Interactable
    {
        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }
        public virtual Quaternion PlayerRotation { get; } = Quaternion.identity;

        protected override void Start()
        {
            base.Start();

            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
            Collider.isTrigger = false;

            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        public sealed override void DoInteraction(Player player) => player.Pickup(this);
        public virtual void Use(Player player) { }
    }
}