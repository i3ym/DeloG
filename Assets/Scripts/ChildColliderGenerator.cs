using System.Collections.Generic;
using UnityEngine;

namespace DeloG
{
    public class ChildColliderGenerator : MonoBehaviour
    {
        [SerializeField]
        string[] AllowedNames = new[] { "коллайдер", "collider" };

        void Awake()
        {
            foreach (var child in GetChildrenRecirsive(transform))
            {
                if (AllowedNames.Length > 0)
                {
                    var lowername = child.name.ToLowerInvariant();

                    bool quit = true;
                    foreach (var name in AllowedNames)
                        if (lowername.Contains(name))
                        {
                            quit = false;
                            break;
                        }

                    if (quit) continue;
                }

                var meshf = child.GetComponent<MeshFilter>();
                if (!meshf) continue;

                GameObject objectTo;
                if (child.parent && child.parent.position == child.position)
                    objectTo = child.parent.gameObject;
                else objectTo = child.gameObject;

                // if (objectTo.GetComponent<Collider>()) continue;

                if (IsMeshBox(meshf.mesh)) objectTo.AddComponent<BoxCollider>();
                else
                {
                    var collider = objectTo.AddComponent<MeshCollider>();
                    collider.convex = true;
                    collider.sharedMesh = meshf.mesh;
                }

                if (objectTo != child.gameObject)
                    Destroy(child.gameObject);
                else
                {
                    Destroy(meshf);
                    Destroy(child.GetComponent<MeshRenderer>());
                }
            }
        }


        static bool IsMeshBox(Mesh mesh)
        {
            return false; // TODO
        }
        static IEnumerable<Transform> GetChildrenRecirsive(Transform obj)
        {
            var childCount = obj.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var tr = obj.GetChild(i);
                yield return tr;

                var children = GetChildrenRecirsive(tr);
                foreach (var child in children)
                    yield return child;
            }
        }
    }
}