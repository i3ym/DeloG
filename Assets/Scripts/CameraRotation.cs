using UnityEngine;

namespace DeloG
{
    public class CameraRotation : MonoBehaviour
    {
        Transform Camera;

        void Start() => Camera = UnityEngine.Camera.main.transform;

        void Update()
        {
            // TODO fix y < 0 rotation
            Camera.localEulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * 2;
        }
    }
}