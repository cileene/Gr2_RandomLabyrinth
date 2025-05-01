using UnityEngine;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker Instance { get; private set; }

    public TextMeshProUGUI timeText;

    private float startTime;
    private bool isTiming;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTiming && timeText != null)
        {
            float currentTime = Time.time - startTime;
            timeText.text = "Time: " + currentTime.ToString("F2") + " seconds";
        }
    }


    public void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
    }

    public void StopTimer()
    {
        if (isTiming)
        {
            float totalTime = Time.time - startTime;
            timeText.text = "Time: " + totalTime.ToString("F2") + " seconds";
            isTiming = false;
        }
    }
}
