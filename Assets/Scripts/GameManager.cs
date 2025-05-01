using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[System.Serializable]
public class HighScoreData
{
    public int highScore;
}
[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string _dataPath;

    // ------------------ VARIABLES ------------------
    [Header("State")] public GameState currentState;

    public enum GameState
    {
        Start,
        Phase1,
        Phase2,
        Won,
        Lost
    }

    [Header("Progression")] public int currentScore;
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
        
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);

        LoadHighScoreFromJson();
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "StartScene") currentState = GameState.Start;
        else if (currentScene == "Labyrinth") currentState = GameState.Phase1;
    }

    private void Update()
    {
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
        if (currentState == GameState.Phase1)
        {
            int random = Random.Range(0, 2);
            
            if (random == 0)
            {
                SceneManager.LoadSceneAsync("Platform1");
            }
            else
            {
                SceneManager.LoadSceneAsync("Platform2");
            }
            
            currentState = GameState.Phase2;
        }
        else if (currentState == GameState.Phase2)
        {
            WinLevel();
        }
    }

    public void WinLevel()
    {
        if (currentState != GameState.Phase2) return;
        currentState = GameState.Won;
        currentScore++;

        SceneManager.LoadSceneAsync("Labyrinth");
        currentState = GameState.Phase1;
    }

    public void LoseGame()
    {
        if (currentState is not (GameState.Phase1 or GameState.Phase2)) return;
        currentState = GameState.Lost;
        

        lastRunTime = _currentRunTime;
        if (currentScore > highScore) highScore = currentScore;
        currentScore = 0;
        SaveHighScoreToJson();

        SceneManager.LoadSceneAsync("LoseScene");
    }
    public void SaveHighScoreToJson()
    {
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");

        HighScoreData highScoreData = new HighScoreData { highScore = highScore };
        string json = JsonUtility.ToJson(highScoreData, true);

        string filePath = Path.Combine(_dataPath, "highScore.json");
        File.WriteAllText(filePath, json);
        Debug.Log("High score saved to: " + filePath);
    }
    public void LoadHighScoreFromJson()
    {
        string filePath = Path.Combine(_dataPath, "highScore.json");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("High score file does not exist!");
            return;
        }

        string text = File.ReadAllText(filePath);
        HighScoreData data = JsonUtility.FromJson<HighScoreData>(text);
        highScore = data.highScore;
        Debug.Log("Loaded high score: " + highScore);
    }
}