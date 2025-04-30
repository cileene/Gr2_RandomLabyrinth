using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the game

public class GameManager : MonoBehaviour
{
    public enum GameState { Start, Playing, Won, Lost } // different possible game states
    public GameState currentState = GameState.Start; // Tracks the state of the game

    public GameObject startScreen; //  UI panel for start
    public GameObject winScreen;   //  UI panel for win
    public GameObject loseScreen;  //  UI panel for lose
  

    void Start()
    {
        ShowStartScreen(); //Shows start UI when game starts
    }

    void Update()
    {
        if (currentState == GameState.Start)
        {
            if (Input.anyKeyDown) 
            {
                StartGame(); // Begins game when player taps screen
            }
        }
       
    }

    void ShowStartScreen()
    {
        startScreen.SetActive(true); // Turns on the start screen panel "Tap to start"
        winScreen.SetActive(false); // Turns off the win screen panel "You Win!
        loseScreen.SetActive(false); //Turns off the lose screen panel "You Lost!"
       
    }

    void StartGame()
    {
       currentState = GameState.Playing;  //  Set game state to Playing
        startScreen.SetActive(false);      //  Hide the start panel
        // (ball script should be actived)
    }

    void WinGame() // Called by BallController script when player wins
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.Won;         // Change game state to Won
        winScreen.SetActive(true);            // Show the win panel
        loseScreen.SetActive(false);          // Makes sure lose panel is hidden
        
    }

    void LoseGame()
    {
         if (currentState != GameState.Playing) return; // Makes sure it only happens when we’re in Playing state (prevents double-triggers)

        currentState = GameState.Lost;        // Changes game state to Lost
        loseScreen.SetActive(true);           // Shows the lose panel
        winScreen.SetActive(false);           // Makes sure win panel is hidden
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

