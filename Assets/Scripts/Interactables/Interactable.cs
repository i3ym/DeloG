using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        protected virtual void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("interactable");
        }

        public abstract void DoInteraction();
    }
}