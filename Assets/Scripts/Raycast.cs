using UnityEngine;

namespace DeloG
{
    public class Raycast : MonoBehaviour
    {
        static readonly int CarLayerMask, InteractableLayerMask;

        static Raycast()
        {
            InteractableLayerMask = LayerMask.GetMask("interactable", "car");
            CarLayerMask = LayerMask.GetMask("car");
        }

        public static bool RaycastInteractable(Transform camera, out RaycastHit hitInfo, float maxDistance = 10) =>
            Physics.Raycast(camera.position, camera.forward, out hitInfo, maxDistance, InteractableLayerMask);
    }
}