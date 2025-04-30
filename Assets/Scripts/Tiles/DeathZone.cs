using UnityEngine;

namespace Tiles
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                //kald game manager for restart 
            }
        }
    }
}
