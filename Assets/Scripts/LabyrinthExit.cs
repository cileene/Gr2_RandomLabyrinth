using UnityEngine;

public class LabyrinthExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player has reached the labyrinths exit!");
        GameManager.Instance.ReachedExit();
    }
}