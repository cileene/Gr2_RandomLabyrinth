using UnityEngine;

public class FallDeathTrigger : MonoBehaviour
{
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if the object falling is the player/ball
        if (other.CompareTag("Player"))
        {
            Debug.Log("death");
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.LoseGame();
            }
        }
    }
}

