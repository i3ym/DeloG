using UnityEngine;

namespace DeloG.Items
{
    public class CameraItem : Item
    {
        [SerializeField] GameObject PhotoPrefab = null;
        [SerializeField] Transform PhotoPositionTransform = null;

        PhotoItem CurrentPhoto;

        public override void Use(Player player)
        {
            if (CurrentPhoto != null)
            {
                if (player.Inventory.Count >= player.Inventory.Capacity) return;

                player.Pickup(CurrentPhoto);
                CurrentPhoto = null;
                return;
            }

            var texture = TakePhoto(player.Camera);
            var photo = Instantiate(PhotoPrefab).GetComponent<PhotoItem>();
            photo.Image = texture;
            photo.GetComponent<Rigidbody>().isKinematic = true;

            photo.transform.SetParent(PhotoPositionTransform);
            photo.transform.localPosition = Vector3.zero;
            photo.transform.localRotation = Quaternion.identity;

            CurrentPhoto = photo;
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