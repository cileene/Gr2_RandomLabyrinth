using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the game

public class GameManager : MonoBehaviour
{
    public enum GameState { Start, Playing, Won, Lost }
    public GameState currentState = GameState.Start;

    public GameObject startScreen; //  UI panel for start
    public GameObject winScreen;   //  UI panel for win
    public GameObject loseScreen;  //  UI panel for lose
    // public ControllerOldSystem ball;    //  ball movement script

    void Start()
    {
        ShowStartScreen();
    }

    void Update()
    {
        if (currentState == GameState.Start)
        {
            if (Input.anyKeyDown) // Start on tap
            {
                StartGame();
            }
        }
        else if (currentState == GameState.Playing)
        {
          //  if (ball.IsInGoalHole) // bool from ball controller script
            {
                WinGame();
            }
          //  else if (ball.IsInWrongHole) // bool from ball controller script
            {
                LoseGame();
            }
        }
    }

    void ShowStartScreen()
    {
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
       
    }

    void StartGame()
    {
       
    }

    void WinGame()
    {
        currentState = GameState.Won;
        
    }

    void LoseGame()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

