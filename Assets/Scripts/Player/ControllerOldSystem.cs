using UnityEngine;

namespace Player
{
    public class ControllerOldSystem : MonoBehaviour
    {
        private Rigidbody _rb;
    
        [SerializeField] private float forceMultiplier = 10f;

        public bool isFlat = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody>(); // Set the Rigidbody to kinematic
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 tilt = Input.acceleration;

            if (isFlat)
            {
                // Map tilt.x to left/right, tilt.y to forward/backward
                tilt = new Vector3(tilt.x, 0f, tilt.y);
            }

            tilt = Vector3.ClampMagnitude(tilt, 1f); // Prevent sudden spikes

            _rb.AddForce(tilt * forceMultiplier);

            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, 5f); // Cap speed
        }
    }
}
