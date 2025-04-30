using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class WalkingPlayerController : MonoBehaviour
    {
        public float moveSpeed = 3f;
        private Rigidbody rb;
        private Vector3 moveDirection = Vector3.zero;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
        }

        public void OnMove(InputValue value)
        {
            moveDirection = value.Get<Vector2>();
        }

    }
}

