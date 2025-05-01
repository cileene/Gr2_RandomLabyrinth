using UnityEngine;

public class LabyrinthExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player has exited the labyrinth!");
        GameManager.Instance.ReachedExit();
    }
}