using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Start, Playing, Won, Lost }
    public GameState currentState = GameState.Start;

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
        SceneManager.LoadScene("PCGTestScene"); //  actual game scene
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

