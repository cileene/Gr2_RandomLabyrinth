using UnityEngine;
using UnityEngine.Events;

public class TileTriggerEvent : MonoBehaviour
{
    public UnityEvent<GameObject> onPlayerEnter;
    public UnityEvent<GameObject> onPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            onPlayerEnter?.Invoke(other.gameObject);
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerExit?.Invoke(other.gameObject);
        }
    }
}

