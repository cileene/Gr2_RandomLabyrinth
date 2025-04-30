using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Add time tracking
//TODO: Add Singleton pattern
//TODO: Make serilaizable
public class GameManager : MonoBehaviour
{
    public enum GameState { Start, Playing, Won, Lost }
    public GameState currentState;

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "StartScene")
        {
            currentState = GameState.Start;
        }
        else if (currentScene == "PCGTestScene")
        {
            currentState = GameState.Playing;
        }
    }

    void Update()
    {
        if (currentState == GameState.Start && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        SceneManager.LoadScene("PCGTestScene");
    }

    public void WinGame()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.Won;
        SceneManager.LoadScene("WinScene");
    }

    public void LoseGame()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.Lost;
        SceneManager.LoadScene("LoseScene");
    }
}