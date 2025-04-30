using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 screenPosition = context.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPosition = hit.point;

                Vector3 direction = (targetPosition - transform.position);
                direction.y = 0f; 
                moveDirection = direction.normalized;
            }
        }
        else if (context.canceled)
        {
            moveDirection = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}

