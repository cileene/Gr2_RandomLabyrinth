using UnityEditor.Build.Content;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //kald game manager start forfra 
        }
    }
}
