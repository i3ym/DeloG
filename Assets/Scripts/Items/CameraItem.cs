using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Items
{
    public class CameraItem : Item
    {
        [SerializeField] GameObject PhotoPrefab = null;

        PhotoItem CurrentItem;
        bool PhotoAnimatonRunning = false;

        public override void Use(Player player)
        {
            if (CurrentItem != null)
            {
                if (PhotoAnimatonRunning) return;
                if (player.Inventory.Count >= player.Inventory.Capacity) return;

                player.Inventory.Pickup(CurrentItem);
                CurrentItem = null;
                return;
            }

            var texture = TakePhoto(player.Camera);
            var photo = Instantiate(PhotoPrefab).GetComponent<PhotoItem>();
            photo.Image = texture;

            photo.transform.SetParent(transform, false);
            photo.transform.position = PhotoPrefab.transform.position;
            photo.transform.rotation = PhotoPrefab.transform.rotation;

            PhotoAnimatonRunning = true;

            StartCoroutine(Animator.Animate(
                () => Animator.MoveToLocal(photo.transform, Vector3.forward * -.3f, 1f, Easing.Linear),
                () => Animator.Action(() => PhotoAnimatonRunning = false)));

            CurrentItem = photo;
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