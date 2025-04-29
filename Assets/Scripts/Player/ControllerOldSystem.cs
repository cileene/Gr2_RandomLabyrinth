using UnityEngine;

namespace Player
{
    public class ControllerOldSystem : MonoBehaviour
    {
        private Rigidbody _rb;
    
        [SerializeField] private float forceMultiplier = 10f;

        public bool isFlat = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _rb = GetComponent<Rigidbody>(); // Set the Rigidbody to kinematic
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Vector3 tilt;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f) //check if wasd is pressed
            {
                tilt = new Vector3(horizontal, 0f, vertical);
            }
            else
            {
                tilt = Input.acceleration;

                if (isFlat)
                {
                    tilt = new Vector3(tilt.x, 0f, tilt.y);
                }
            }

            tilt = Vector3.ClampMagnitude(tilt, 1f); // Prevent sudden spikes

            _rb.AddForce(tilt * forceMultiplier);

            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, 5f); // Cap speed
        }
    }
}
