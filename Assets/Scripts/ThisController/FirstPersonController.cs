namespace ThisController
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private int jumpForce = 50;
        [SerializeField, Range(0, 10)] private float moveSpeed = 5;
        [SerializeField, Range(1, 500)] private int lookSpeed = 100;

        private Camera _mainCamera;
        private Rigidbody _rigidbody;
        private Vector2 _lookInput, _moveInput;
        private bool _isJumping, _isGrounded;

        private void Start()
        {
            _mainCamera = Camera.main;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            CheckGrounded();
            HandleMove();
            HandleJump();
            HandleLook();
        }
        
        private void CheckGrounded()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        }
        
        private void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }
        
        private void OnLook(InputValue value)
        {
            _lookInput = value.Get<Vector2>();
        }
        
        private void OnJump(InputValue value)
        {
            if (!_isGrounded) return;
            _isJumping = value.isPressed;
        }

        private void HandleMove()
        {
            var moveDirection = transform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y));
            var targetPosition = _rigidbody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(targetPosition);
        }

        private void HandleLook()
        {
            transform.Rotate(Vector3.up * (_lookInput.x * lookSpeed * Time.fixedDeltaTime));
            _mainCamera.transform.Rotate(Vector3.left * (_lookInput.y * lookSpeed * Time.fixedDeltaTime));
        }

        private void HandleJump()
        {
            if (!_isJumping) return;
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isJumping = false;
        }
    }
}