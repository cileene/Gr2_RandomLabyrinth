using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    public Transform spawnPoint;
    public AudioClip gameMusicClip;

    void Start()
    {
        Respawn(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallZone"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = spawnPoint.position;

        TimeTracker.Instance.StartTimer();
        SoundManager.Instance.PlayMusic(gameMusicClip);

    }


}
