using UnityEngine;

namespace DeloG.Items
{
    public class PhotoItem : Item
    {
        public override Quaternion PlayerRotation { get; } = Quaternion.Euler(-80, 0, 0);

        public Texture Image
        {
            get => ImageRenderer.material.mainTexture;
            set
            {
                ImageRenderer.material.mainTexture = value;

                float min = Mathf.Min(value.width, value.height);
                transform.localScale = new Vector3(value.width / min, 1, value.height / min);
            }
        }

        Renderer ImageRenderer;

        void Awake() => ImageRenderer = GetComponent<Renderer>();
    }
}