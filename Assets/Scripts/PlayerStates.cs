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
                GameObject.Destroy(player.gameObject);
            }
            else if (collision.gameObject.CompareTag("Water"))
            {
                Debug.Log("Normal player picked up water.");
                player.SetState(new WaterState());
            }
        }
    }
        
    public class WaterState : IPlayerStates
    {
        public void OnCollision(PlayerStateManager player, Collision collision)
        {
            if (collision.gameObject.CompareTag("Fire"))
            {
                Debug.Log("Water player extinguishes fire.");
                GameObject.Destroy(collision.gameObject); // Destroy fire
                player.SetState(new NormalState()); // Optional: revert to normal
            }
        }
    }

}