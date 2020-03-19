using UnityEngine;

namespace DeloG
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