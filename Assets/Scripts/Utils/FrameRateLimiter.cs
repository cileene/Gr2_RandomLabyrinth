using UnityEngine;

// This script is used to limit the frame rate of the game to a specific value
// Just connect this script to a game object
namespace Utils
{
    public class FrameRateLimiter : MonoBehaviour
    {
        [Tooltip("The target frame rate, 0 = no limit")]
        [SerializeField] private int targetFrameRate = 60;

        [Tooltip("Turn vSync on or off")]
        [SerializeField] private bool vSync;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSync ? 1 : 0;
        }
    }
}