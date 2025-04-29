using UnityEngine;

public class ControllerOldSystem : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private float forceMultiplier = 10f;

    public bool isFlat = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Set the Rigidbody to kinematic
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tilt = Input.acceleration;
        if (isFlat)
        {
            tilt = Quaternion.Euler(90, 0, 0) * tilt;
        }
        
        rb.AddForce(tilt * forceMultiplier);
    }
}
