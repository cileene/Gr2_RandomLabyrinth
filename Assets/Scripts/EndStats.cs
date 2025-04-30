using UnityEngine;
using TMPro;

public class EndStats : MonoBehaviour
{
    [SerializeField] private TMP_Text runTime;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text highScore;
    
    private void Start()
    {
        var gm = GameManager.Instance;

        if (runTime != null && gm != null)
            runTime.text = $"Run time: {gm.lastRunTime:F2}";

        if (score != null && gm != null)
            score.text = $"Score: {gm.currentScore}";

        if (highScore != null && gm != null)
            highScore.text = $"High Score: {gm.highScore}";
    }
}