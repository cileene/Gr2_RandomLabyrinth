using UnityEngine;
using UnityEngine.Events;

namespace TurtleTileIINT
{
    public class TurtleController : MonoBehaviour 
    {
        public UnityEvent<string> onTileEntered;

        public IMovementStrategy CurrentStrategy;

        private void Update()
        {
            CurrentStrategy?.Move(this);
        }

        public void SetStrategy(IMovementStrategy strategy, string tileType) 
        {
            CurrentStrategy = strategy;
            onTileEntered?.Invoke(tileType);
        }

        public void MoveWithSpeed(float speed) 
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
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

