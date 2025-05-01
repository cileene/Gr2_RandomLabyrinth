using UnityEngine;

public class SpawnRandomPrefab : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabPool;

    private void Start()
    {
        // Check if prefabPool is not empty
        if (prefabPool.Length == 0)
        {
            Debug.LogWarning("Prefab pool is empty. No prefab will be spawned.");
            return;
        }
        // Generate a random index
        int randomIndex = Random.Range(0, prefabPool.Length);
        // Instantiate the prefab at the current position and rotation
        GameObject randomPrefab = Instantiate(prefabPool[randomIndex], transform.position, transform.rotation);
        // Set the parent of the instantiated prefab to this object
        randomPrefab.transform.SetParent(transform);
    }
}