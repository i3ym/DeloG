using UnityEngine;

namespace DeloG
{
    public class Map : MonoBehaviour
    {
        void Awake()
        {
            foreach (var children in transform.GetComponentsInChildren<Renderer>())
                children.gameObject.AddComponent<MeshCollider>();
        }
    }
}