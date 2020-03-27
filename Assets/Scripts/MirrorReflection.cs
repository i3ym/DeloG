using UnityEngine;
using System.Collections;

namespace DeloG
{
    [ExecuteInEditMode]
    public class MirrorReflection : MonoBehaviour
    {
        static bool InsideRendering = false;

        [SerializeField] bool DisablePixelLights = true;
        [SerializeField] int TextureSize = 256;
        [SerializeField] float ClipPlaneOffset = 0.07f;

        [SerializeField] LayerMask ReflectLayers = -1;
        Hashtable ReflectionCameras = new Hashtable();

        RenderTexture ReflectionTexture;
        int OldReflectionTextureSize;

        Renderer Renderer;

        void Start()
        {
            Renderer = GetComponent<Renderer>();
        }

        void OnWillRenderObject()
        {
            if (!enabled || !Renderer || !Renderer.sharedMaterial || !Renderer.enabled)
                return;

            Camera cam = Camera.current;
            if (!cam) return;

            if (InsideRendering) return;
            InsideRendering = true;

            Camera reflectionCamera;
            CreateMirrorObjects(cam, out reflectionCamera);

            int oldPixelLightCount = QualitySettings.pixelLightCount;
            if (DisablePixelLights) QualitySettings.pixelLightCount = 0;

            UpdateCameraModes(cam, reflectionCamera);

            var normal = transform.up;

            // Render reflection
            // Reflect camera around reflection plane
            float d = -Vector3.Dot(normal, transform.position) - ClipPlaneOffset;
            var reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

            var reflection = Matrix4x4.zero;
            CalculateReflectionMatrix(ref reflection, reflectionPlane);

            var oldpos = cam.transform.position;
            var newpos = reflection.MultiplyPoint(oldpos);
            reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;

            // Setup oblique projection matrix so that near plane is our reflection
            // plane. This way we clip everything below/above it for free.
            Vector4 clipPlane = CameraSpacePlane(reflectionCamera, transform.position, normal, 1.0f);
            //Matrix4x4 projection = cam.projectionMatrix;
            Matrix4x4 projection = cam.CalculateObliqueMatrix(clipPlane);
            reflectionCamera.projectionMatrix = projection;

            reflectionCamera.cullingMask = ~(1 << 4) & ReflectLayers.value; // never render water layer
            reflectionCamera.targetTexture = ReflectionTexture;
            GL.invertCulling = true;
            reflectionCamera.transform.position = newpos;
            Vector3 euler = cam.transform.eulerAngles;
            reflectionCamera.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
            reflectionCamera.Render();
            reflectionCamera.transform.position = oldpos;
            GL.invertCulling = false;

            foreach (Material mat in Renderer.sharedMaterials)
                if (mat.HasProperty("_ReflectionTex"))
                    mat.SetTexture("_ReflectionTex", ReflectionTexture);

            if (DisablePixelLights)
                QualitySettings.pixelLightCount = oldPixelLightCount;

            InsideRendering = false;
        }

        void UpdateCameraModes(Camera src, Camera dest)
        {
            if (dest == null)
                return;
            // set camera to clear the same way as current camera
            dest.clearFlags = src.clearFlags;
            dest.backgroundColor = src.backgroundColor;
            if (src.clearFlags == CameraClearFlags.Skybox)
            {
                Skybox sky = src.GetComponent(typeof(Skybox)) as Skybox;
                Skybox mysky = dest.GetComponent(typeof(Skybox)) as Skybox;
                if (!sky || !sky.material)
                {
                    mysky.enabled = false;
                }
                else
                {
                    mysky.enabled = true;
                    mysky.material = sky.material;
                }
            }
            // update other values to match current camera.
            // even if we are supplying custom camera&projection matrices,
            // some of values are used elsewhere (e.g. skybox uses far plane)
            dest.farClipPlane = src.farClipPlane;
            dest.nearClipPlane = src.nearClipPlane;
            dest.orthographic = src.orthographic;
            dest.fieldOfView = src.fieldOfView;
            dest.aspect = src.aspect;
            dest.orthographicSize = src.orthographicSize;
        }
        void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
        {
            reflectionCamera = null;

            // Reflection render texture
            if (!ReflectionTexture || OldReflectionTextureSize != TextureSize)
            {
                if (ReflectionTexture)
                    DestroyImmediate(ReflectionTexture);
                ReflectionTexture = new RenderTexture(TextureSize, TextureSize, 16);
                ReflectionTexture.name = "__MirrorReflection" + GetInstanceID();
                ReflectionTexture.isPowerOfTwo = true;
                ReflectionTexture.hideFlags = HideFlags.DontSave;
                OldReflectionTextureSize = TextureSize;
            }

            // Camera for reflection
            reflectionCamera = ReflectionCameras[currentCamera] as Camera;
            if (!reflectionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
            {
                var go = new GameObject("Mirror Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
                reflectionCamera = go.GetComponent<Camera>();
                reflectionCamera.enabled = false;
                reflectionCamera.transform.position = transform.position;
                reflectionCamera.transform.rotation = transform.rotation;
                reflectionCamera.gameObject.AddComponent<FlareLayer>();
                go.hideFlags = HideFlags.HideAndDontSave;
                ReflectionCameras[currentCamera] = reflectionCamera;
            }
        }

        static float Sign(float a)
        {
            if (a > 0) return 1;
            if (a < 0) return -1;
            return 0;
        }
        Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
        {
            var m = cam.worldToCameraMatrix;
            var cnormal = m.MultiplyVector(normal).normalized * sideSign;

            return new Vector4(cnormal.x, cnormal.y, cnormal.z,
                -Vector3.Dot(m.MultiplyPoint(pos + normal * ClipPlaneOffset), cnormal));
        }

        void OnDisable()
        {
            if (ReflectionTexture)
            {
                DestroyImmediate(ReflectionTexture);
                ReflectionTexture = null;
            }
            foreach (DictionaryEntry kvp in ReflectionCameras)
                DestroyImmediate(((Camera) kvp.Value).gameObject);
            ReflectionCameras.Clear();
        }

        static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
            reflectionMat.m01 = (-2F * plane[0] * plane[1]);
            reflectionMat.m02 = (-2F * plane[0] * plane[2]);
            reflectionMat.m03 = (-2F * plane[3] * plane[0]);

            reflectionMat.m10 = (-2F * plane[1] * plane[0]);
            reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
            reflectionMat.m12 = (-2F * plane[1] * plane[2]);
            reflectionMat.m13 = (-2F * plane[3] * plane[1]);

            reflectionMat.m20 = (-2F * plane[2] * plane[0]);
            reflectionMat.m21 = (-2F * plane[2] * plane[1]);
            reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
            reflectionMat.m23 = (-2F * plane[3] * plane[2]);

            reflectionMat.m30 = 0F;
            reflectionMat.m31 = 0F;
            reflectionMat.m32 = 0F;
            reflectionMat.m33 = 1F;
        }
    }
}