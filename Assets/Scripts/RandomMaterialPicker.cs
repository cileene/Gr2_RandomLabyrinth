using UnityEngine;


public class RandomMaterialPicker : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private void Start()
    {
        if (materials.Length == 0)
        {
            Debug.LogWarning("No materials assigned to RandomMaterialPicker.");
            return;
        }

        int randomIndex = Random.Range(0, materials.Length);
        GetComponent<Renderer>().material = materials[randomIndex];
    }
}