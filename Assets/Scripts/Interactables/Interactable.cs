using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] ColliderCreation CreateCollider = ColliderCreation.DontCreate;

        Renderer Renderer;
        bool DoStopCoroutine = false;

        protected virtual void Awake()
        {
            Renderer = GetComponent<Renderer>();

            gameObject.layer = LayerMask.NameToLayer("interactable");
            // gameObject.GetComponent<Collider>().isTrigger = true;

            if (CreateCollider != ColliderCreation.DontCreate)
            {
                if (CreateCollider == ColliderCreation.CreateBox)
                    gameObject.AddComponent<BoxCollider>();
                else if (CreateCollider == ColliderCreation.CreateMesh)
                {
                    var collider = gameObject.AddComponent<MeshCollider>();
                    collider.convex = true;
                    collider.sharedMesh = GetComponent<MeshFilter>().mesh;
                }
            }
            else if (!GetComponent<Collider>())
                Debug.LogWarning("Object " + name + " is Interactable, but doesn't has any Collider", this);
        }

        public abstract void DoInteraction(Player player);

        public void StartHighlighting()
        {
            if (!Renderer) return;

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


        enum ColliderCreation : byte { DontCreate, CreateMesh, CreateBox }
    }
}