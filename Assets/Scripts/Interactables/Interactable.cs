using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    [RequireComponent(typeof(Collider), typeof(Renderer))]
    public abstract class Interactable : MonoBehaviour
    {
        protected Renderer Renderer { get; private set; }
        bool DoStopCoroutine = false;

        protected virtual void Start()
        {
            Renderer = GetComponent<Renderer>();

            gameObject.layer = LayerMask.NameToLayer("interactable");
            gameObject.GetComponent<Collider>().isTrigger = true;
        }

        public abstract void DoInteraction(Player player);

        public void StartHighlighting()
        {
            StopHighlighting();
            StartCoroutine(HighlightEnumerator());
        }
        public void StopHighlighting() => DoStopCoroutine = true;

        IEnumerator HighlightEnumerator()
        {
            DoStopCoroutine = false;
            var startColor = Renderer.material.color;
            var stime = Time.time;

            while (!DoStopCoroutine)
            {
                Renderer.material.color = startColor * ((Mathf.Sin((Time.time - stime) * 5 + (3 * Mathf.PI / 2)) + 2) * 2);
                yield return null;
            }

            Renderer.material.color = startColor;
        }
    }
}