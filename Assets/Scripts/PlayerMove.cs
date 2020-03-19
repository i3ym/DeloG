using UnityEngine;

namespace DeloG
{
    public class PlayerMove : MonoBehaviour
    {
        const float WalkingSpeed = 10;

        Camera Camera;
        Rigidbody Rigidbody;

        void Start()
        {
            Camera = UnityEngine.Camera.main;
            Rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Rigidbody.velocity =
                new Vector3(Camera.transform.forward.x, 0, Camera.transform.forward.z).normalized * Input.GetAxis("Vertical") * WalkingSpeed
                + Camera.transform.right * Input.GetAxis("Horizontal") * WalkingSpeed
                + new Vector3(0, Rigidbody.velocity.y, 0);
        }
    }
}