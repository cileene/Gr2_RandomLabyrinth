using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimeTracker.Instance.StopTimer();

            if (SoundManager.Instance.endZoneSound != null)
            {
                SoundManager.Instance.PlaySfx(SoundManager.Instance.endZoneSound);
            }
        }
    }
}
