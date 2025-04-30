using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Controls : MonoBehaviour
    {
        public InputActionAsset inputActions;
        public float speed = 10f;

        private Rigidbody rb;
        private InputAction moveAction;
        private Vector3 movementInput;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();

            // Get reference to the 'Move' action
            var map = inputActions.FindActionMap("Player");
            moveAction = map.FindAction("Move");

            moveAction.performed += OnMove;
            moveAction.canceled += ctx => movementInput = Vector3.zero;
        }

        private void OnEnable()
        {
            moveAction.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector3>();
        }

        void FixedUpdate()
        {
            Vector3 force = new Vector3(movementInput.x, 0, movementInput.y); // z = forward
            rb.AddForce(force * speed);
        }
    }
}
