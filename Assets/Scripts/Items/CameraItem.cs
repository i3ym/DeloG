using UnityEngine;

namespace DeloG.Items
{
    [RequireComponent(typeof(ItemHolder))]
    public class CameraItem : Item
    {
        [SerializeField] GameObject PhotoPrefab = null;

        ItemHolder Inventory;

        protected override void Awake()
        {
            base.Awake();
            Inventory = GetComponent<ItemHolder>();
        }

        public override void Use(Player player)
        {
            if (Inventory.CurrentItem != null)
            {
                if (player.Inventory.Count >= player.Inventory.Capacity) return;

                var item = Inventory.CurrentItem;
                Inventory.RemoveItem(item);
                player.Inventory.Pickup(item);
                return;
            }

            var texture = TakePhoto(player.Camera);
            var photo = Instantiate(PhotoPrefab).GetComponent<PhotoItem>();
            photo.transform.localScale /= 2f;
            photo.Image = texture;

            photo.transform.SetParent(transform);
            photo.transform.localPosition = Vector3.zero;
            photo.transform.localRotation = Inventory.LocalTargerRotation;

            Inventory.Pickup(photo);
        }

        public Texture2D TakePhoto(Camera camera)
        {
            var rt = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = rt;

            var screenshot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            camera.Render();

            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, screenshot.width, screenshot.height), 0, 0);
            screenshot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            return screenshot;
        }
    }
}