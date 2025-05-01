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
            timeText.text = "Tid: " + totalTime.ToString("F2") + " sekunder";
            isTiming = false;
        }
    }
}
