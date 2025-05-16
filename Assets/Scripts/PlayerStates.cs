using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public interface IPlayerStates // Interface for player states
    {
        public void HandleCollision(PlayerStateManager player, Collision collision);
        public void HandleTrigger(PlayerStateManager player, Collider other);
    }
        
    public class NormalState : IPlayerStates // Normal state of the player
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
                SoundManager.Instance.PlaySfx(SoundManager.Instance.waterDrip);
                player.SetState(new WaterState());
                player.wetParticles.SetActive(true); // Activate wet particles
            }
        }
    }
        
    public class WaterState : IPlayerStates // Water state of the player
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
                SoundManager.Instance.PlaySfx(SoundManager.Instance.fireExtinguishing);
                Destroy(other.gameObject.GetComponentInChildren<ParticleSystem>().gameObject); // Destroy fire
                player.wetParticles.SetActive(false); // Activate wet particles
                player.SetState(new NormalState()); // Optional: revert to normal
            }
        }
    }
}