using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        protected virtual void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("interactable");
            gameObject.GetComponent<Collider>().isTrigger = true;
        }

        public abstract void DoInteraction();
    }
}