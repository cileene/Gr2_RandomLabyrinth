using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ------------------ VARIABLES ------------------
    [Header("State")]
    public GameState currentState;
    public enum GameState { Start, Phase1, Phase2, Won, Lost }
    
    [Header("Progression")]
    public int currentScore;
    public int highScore;
    public float lastRunTime;
   
    private float _currentRunTime;
    
    
    // ------------------ METHODS ------------------
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "StartScene") currentState = GameState.Start;
        else if (currentScene == "Labyrinth") currentState = GameState.Phase1;
    }

    private void Update()
    {
        if (currentState == GameState.Start && Input.anyKeyDown) StartGame();
        if (currentState is GameState.Phase1 or GameState.Phase2) _currentRunTime += Time.deltaTime;
    }

    public void StartGame()
    {
        _currentRunTime = 0f;
        currentState = GameState.Phase1;
        SceneManager.LoadSceneAsync("Labyrinth");
    }
    
    public void ReachedExit()
    {
        if (currentState != GameState.Phase1) return;
        
        currentState = GameState.Phase2;
        SceneManager.LoadSceneAsync("Labyrinth");
    }

    public void WinLevel()
    {
        if (currentState != GameState.Phase2) return;
        currentScore++;

        currentState = GameState.Won;
        SceneManager.LoadSceneAsync("WinScene");
    }

    public void LoseGame()
    {
        if (currentState is not (GameState.Phase1 or GameState.Phase2)) return;
        currentState = GameState.Lost;
        
        lastRunTime = _currentRunTime;
        if (currentScore > highScore) highScore = currentScore;

        SceneManager.LoadSceneAsync("LoseScene");
    }
}