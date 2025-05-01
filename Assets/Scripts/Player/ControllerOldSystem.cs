using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class ControllerOldSystem : MonoBehaviour
    {
        private Rigidbody _rb;
        
        public float moveSpeed = 1f;
    
        [SerializeField] private float forceMultiplier = 10f;
        public bool isFlat = true;

        private Vector3 currentDirection = Vector3.zero;  // To store the current tilt direction

        private void Start()
        {
            _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        }

        private void FixedUpdate()
        {
            // Check if there is any touch input
            if (Input.touchCount == 0)
            {
                
                // No touch, use tilt or keyboard input
                Vector3 tilt;

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                // If keyboard input is detected, use it
                if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f)
                {
                    tilt = new Vector3(horizontal, 0f, vertical);
                    currentDirection = tilt;  // Update the tilt direction when keyboard input is used
                }
                else
                {
                    tilt = Input.acceleration;  // Use accelerometer (tilt)

                    if (isFlat)
                    {
                        tilt = new Vector3(tilt.x, 0f, tilt.y);  // Flatten Y-axis if needed
                    }

                    // Store the tilt direction while no touch is active
                    currentDirection = Vector3.ClampMagnitude(tilt, 1f);
                }

                // Apply force based on tilt direction
                _rb.AddForce(currentDirection * forceMultiplier);

                // Cap speed
                _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, 5f);
            }
            else
            {
                _rb.linearVelocity = Vector3.zero; // Reset force when touch is detected
                _rb.MovePosition(_rb.position + currentDirection * (moveSpeed * Time.fixedDeltaTime));
            }
        }
        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            currentDirection = new Vector3(input.x, 0f, input.y);
        }
         
    }
}