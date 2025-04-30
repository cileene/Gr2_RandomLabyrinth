using UnityEngine;

namespace Utils
{
    public class PhysicsSettings : MonoBehaviour
    {
        [Tooltip("Set the multiplier to adjust the gravity")]
        public float gravityMultiplier = 1f;
        [Tooltip("Select the desired fixed update frames per second")]
        public FPS fixedUpdateFPS = FPS.FPS120;
    
        private float _baseGravity = -9.81f;
        private float _calculatedGravity;
    
        public enum FPS
        {
            FPS30 = 30,
            FPS45 = 45,
            FPS50 = 50,
            FPS60 = 60,
            FPS90 = 90,
            FPS120 = 120,
            FPS240 = 240
        }
    
        private void Start()
        {
            // Set the fixed timestep to 1/120 seconds (120 updates per second)
            Time.fixedDeltaTime = 1f / (int)fixedUpdateFPS;
        
            // Set the physics gravity to the desired value
            _calculatedGravity = _baseGravity * gravityMultiplier;
            Physics.gravity = new Vector3(0, _calculatedGravity, 0); // Adjust as needed
        }
    }
}
