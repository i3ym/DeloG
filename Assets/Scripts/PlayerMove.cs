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
            var speed = Input.GetKey(KeyCode.LeftShift) ? WalkingSpeed * 1.5f : WalkingSpeed;

            Rigidbody.velocity =
                new Vector3(Camera.transform.forward.x, 0, Camera.transform.forward.z).normalized * Input.GetAxis("Vertical") * speed
                + Camera.transform.right * Input.GetAxis("Horizontal") * speed
                + new Vector3(0, Rigidbody.velocity.y, 0);
        }
    }
}