using UnityEngine;
using TMPro;

public class EndStats : MonoBehaviour // display stats at the end of the game
{
    [SerializeField] private TMP_Text runTime;
    //[SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text highScore;
    
    private void Start()
    {
        var gm = GameManager.Instance;

        if (runTime != null && gm != null && gm.lastRunTime > 0)
            runTime.text = $"Last Run: {gm.lastRunTime:F2}";
        else if (runTime != null && gm != null)
            runTime.text = $"You haven't finished yet!";

        //if (score != null && gm != null)
           // score.text = $"Score: {gm.currentScore}";

        if (highScore != null && gm != null)
            highScore.text = $"Fastest Run: {gm.fastestRunTime}";
    }
}