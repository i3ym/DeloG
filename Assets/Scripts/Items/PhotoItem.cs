using UnityEngine;

namespace DeloG.Items
{
    public class PhotoItem : Item
    {
        public override Quaternion PlayerRotation { get; } = Quaternion.Euler(-160, 0, 0);

        public Texture Image
        {
            get => PhotoMaterial.mainTexture;
            set => PhotoMaterial.mainTexture = value;
        }

        [SerializeField] Material PhotoMaterial = null;

        protected override void Awake()
        {
            base.Awake();
            if (PhotoMaterial == null) PhotoMaterial = GetComponent<Renderer>().material;
        }
    }
}