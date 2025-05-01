using UnityEngine;
using UnityEngine.Events;

namespace TurtleTileIINT
{
    public class TurtleController : MonoBehaviour 
    {
        public UnityEvent<string> OnTileEntered;

        public IMovementStrategy currentStrategy;

        public void SetStrategy(IMovementStrategy strategy, string tileType) 
        {
            currentStrategy = strategy;
            OnTileEntered?.Invoke(tileType);
        }

        public void MoveWithSpeed(float speed) 
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void EnableSmoke() 
        {
            // Trigger smoke animation
        }

        public void DisableSmoke() 
        {
            // Stop smoke animation
        }

        public void DisableMovement() 
        {
            // Freeze movement
        }

        public void PlayBubbleFloatAnim() 
        {
            // Play float animation
        }

        public void StopBubbleAnim() 
        {
            // Stop bubble animation
        }
    }
}

