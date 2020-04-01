using UnityEngine;

namespace DeloG
{
    public class MirrorMaterial : MonoBehaviour
    {
        void Awake()
        {
            var renderer = GetComponent<Renderer>();
            renderer.material = Instantiate(renderer.sharedMaterial);
        }
    }
}