using UnityEngine;

namespace Platformer
{
    public class EndZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                TimeTracker.Instance.StopTimer();

                if (SoundManager.Instance.endZoneSound != null)
                {
                    SoundManager.Instance.PlaySfx(SoundManager.Instance.endZoneSound);
                }
                GameManager.Instance.ReachedExit();
            }
        }
    }
}
