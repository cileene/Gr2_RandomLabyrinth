using UnityEngine;

namespace Tiles
{
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
}
