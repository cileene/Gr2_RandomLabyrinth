using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        private Rigidbody _rb;
        private Vector3 _currentDirection = Vector3.zero;  // To store the current tilt direction
        public float moveSpeed = 1f;
        [SerializeField] private float forceMultiplier = 10f;
        public bool isFlat = true;

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }
        

        private void Start()
        {
            _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        }

        private void FixedUpdate()
        {
            // New Input System only
            bool isTouching = Touchscreen.current != null &&
                              Touchscreen.current.primaryTouch.press.isPressed;

            if (isTouching)
            {
                // Drag mode
                _rb.linearVelocity = Vector3.zero;
                _rb.MovePosition(_rb.position + _currentDirection * (moveSpeed * Time.fixedDeltaTime));
                return;
            }

            // Determine direction from OnMove input or accelerometer
            Vector3 dir = Vector3.zero;
            if (_currentDirection.sqrMagnitude > 0.001f)
            {
                dir = _currentDirection;
            }
            else if (Accelerometer.current != null)
            {
                dir = Accelerometer.current.acceleration.ReadValue();
                if (isFlat)
                    dir = new Vector3(dir.x, 0f, dir.y);
            }

            dir = Vector3.ClampMagnitude(dir, 1f);

            // Apply force and clamp speed
            _rb.AddForce(dir * (forceMultiplier * Time.fixedDeltaTime), ForceMode.Impulse);
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, 5f);
        }
        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            _currentDirection = new Vector3(input.x, 0f, input.y);
        }
        
    }
}