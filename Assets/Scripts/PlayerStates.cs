using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public interface IPlayerStates
    {
        void HandleCollision(PlayerStateManager player, Collision collision);
        void HandleTrigger(PlayerStateManager player, Collider other);
    }
        
    public class NormalState : IPlayerStates
    {
        public void HandleTrigger(PlayerStateManager player, Collider other)
        {
            if (other.gameObject.CompareTag("Fire"))
            {
                Debug.Log("Normal player hit fire and dies.");
                GameManager.Instance.LoseGame(); // Call the GameManager to restart the game
            }
            
        }
        public void HandleCollision(PlayerStateManager player, Collision collision)
        {
            if (collision.gameObject.CompareTag("Water"))
            {
                Debug.Log("Normal player picked up water.");
                player.SetState(new WaterState());
                player.wetParticles.SetActive(true); // Activate wet particles
            }
        }
    }
        
    public class WaterState : IPlayerStates
    {
        public void HandleCollision(PlayerStateManager player, Collision collision)
        {
            if (collision.gameObject.CompareTag("Fire"))
            {
                Debug.Log("Oh no!");
            }
        }
        public void HandleTrigger(PlayerStateManager player, Collider other)
        {
            if (other.gameObject.CompareTag("Fire"))
            {
                Debug.Log("Water player extinguishes fire.");
                Destroy(other.gameObject.GetComponentInChildren<ParticleSystem>().gameObject); // Destroy fire
                player.wetParticles.SetActive(false); // Activate wet particles
                player.SetState(new NormalState()); // Optional: revert to normal
            }
        }
    }
}