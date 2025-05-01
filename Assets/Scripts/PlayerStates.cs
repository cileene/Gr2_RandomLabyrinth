using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public interface IPlayerStates
    {
        void OnCollision(PlayerStateManager player, Collision collision);
    }
        
    public class NormalState : IPlayerStates
    {
        public void OnCollision(PlayerStateManager player, Collision collision)
        {
            if (collision.gameObject.CompareTag("Fire"))
            {
                Debug.Log("Normal player hit fire and dies.");
                GameManager.Instance.LoseGame(); // Call the GameManager to restart the game
            }
            else if (collision.gameObject.CompareTag("Water"))
            {
                Debug.Log("Normal player picked up water.");
                player.SetState(new WaterState());
                //player.gameObject.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(true);
            }
        }
    }
        
    public class WaterState : IPlayerStates
    {
        public void OnCollision(PlayerStateManager player, Collision collision)
        {
            if (collision.gameObject.CompareTag("Fire"))
            {
                //player.gameObject.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
                Debug.Log("Water player extinguishes fire.");
                Destroy(collision.gameObject.GetComponentInChildren<ParticleSystem>().gameObject); // Destroy fire
                player.SetState(new NormalState()); // Optional: revert to normal
            }
        }
    }

}