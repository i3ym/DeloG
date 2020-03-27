using DeloG.Interactables;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Items
{
    public class PhotoBoard : Interactable
    {
        public override void DoInteraction(Player player)
        {
            if (!(player.CurrentItem is PhotoItem photo)) return;

            var raycast = Raycast.RaycastInteractable(player.Camera.transform, out var hit);
            if (!raycast) return;

            player.DropCurrentItem();
            photo.Rigidbody.isKinematic = true;
            photo.Collider.isTrigger = true;
            photo.transform.SetParent(transform);

            StartCoroutine(Animator.Animate(
                new[]
                {
                    Animator.MoveToWorld(photo.transform, hit.point, .7f, Easing.OutCubic),
                    Animator.RotateToWorld(photo.transform, Quaternion.Euler(hit.normal), .7f, Easing.OutCubic)
                }));
        }
    }
}