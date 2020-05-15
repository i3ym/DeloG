using UnityEngine;

namespace DeloG
{
    public class Map : MonoBehaviour
    {
        void Awake()
        {
            // TODO model: generate colliders: true
            foreach (var children in transform.GetComponentsInChildren<Renderer>())
                children.gameObject.AddComponent<MeshCollider>();
        }
    }
}