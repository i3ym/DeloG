using UnityEngine;

namespace DeloG.Items
{
    public class PhotoItem : Item
    {
        public Texture Image
        {
            get => PhotoMaterial.mainTexture;
            set => PhotoMaterial.mainTexture = value;
        }

        [SerializeField] Material PhotoMaterial = null;
    }
}