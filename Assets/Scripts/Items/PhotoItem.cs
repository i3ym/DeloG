using UnityEngine;

namespace DeloG.Items
{
    public class PhotoItem : Item
    {
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

        protected override void Awake()
        {
            base.Awake();
            ImageRenderer = GetComponent<Renderer>();
        }
    }
}