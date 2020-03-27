using UnityEngine;

namespace DeloG.Items
{
    public class CameraItem : Item
    {
        [SerializeField] GameObject PhotoPrefab = null;

        public override void Use(Player player)
        {
            var texture = TakePhoto(player.Camera);
            var photo = Instantiate(PhotoPrefab);
            photo.GetComponent<PhotoItem>().Image = texture;

            photo.transform.position = player.transform.position;
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