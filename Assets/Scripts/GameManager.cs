using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using Random = UnityEngine.Random;

[System.Serializable]
public class RunTimeData
{
    public float fastestRunTime;
}
[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string _dataPath; // Path to save the JSON file

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

    [Header("Progression")] 
    //public int currentScore;
    //public int highScore;
    public float lastRunTime;
    public float fastestRunTime;

    private float _currentRunTime;

    // ------------------ METHODS ------------------
    private void Awake() 
    {
        if (Instance == null) // singleton pattern to ensure only one instance of GameManager exists
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        _dataPath = Application.persistentDataPath + "/Player_Data/"; // Path to save the JSON file
        Debug.Log(_dataPath);

        LoadRunTimeFromJson(); // Load the last run time from JSON
        
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        LoadRunTimeFromJson();

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
        SoundManager.Instance.PlayMusic(SoundManager.Instance.gameMusicClip);
        AnalyticsService.Instance.RecordEvent("startGame");
        //LoadRunTimeFromJson();
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
            //SaveRunTimeToJson();
            WinLevel();
        }
    }

    public void WinLevel()
    {
        if (currentState != GameState.Phase2) return;
        currentState = GameState.Won;
        //currentScore++;
        //LoadRunTimeFromJson();
        lastRunTime = _currentRunTime;
        if (fastestRunTime == 0 || _currentRunTime < fastestRunTime) fastestRunTime = _currentRunTime;
        
        SaveRunTimeToJson();
        UGSSnitch();
        
        AnalyticsService.Instance.Flush();
        
        SceneManager.LoadSceneAsync("WinScene");
        
        AnalyticsService.Instance.RecordEvent("winLevel");
    }

    public void LoseGame()
    {
        if (currentState is not (GameState.Phase1 or GameState.Phase2)) return;
        currentState = GameState.Lost;
        

        //if (currentScore > highScore) highScore = currentScore;
        //currentScore = 0;
        //LoadRunTimeFromJson();

        SceneManager.LoadSceneAsync("LoseScene");
        AnalyticsService.Instance.RecordEvent("loseGame");
    }
    public void SaveRunTimeToJson()
    {
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");

        RunTimeData runTimeData = new RunTimeData { fastestRunTime = fastestRunTime };
        string json = JsonUtility.ToJson(runTimeData, true);

        string filePath = Path.Combine(_dataPath, "runTime.json");
        File.WriteAllText(filePath, json);
        Debug.Log("Run time saved to: " + filePath);
    }
    public void LoadRunTimeFromJson()
    {
        string filePath = Path.Combine(_dataPath, "runTime.json");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Run time file does not exist!");
            return;
        }

        string text = File.ReadAllText(filePath);
        RunTimeData data = JsonUtility.FromJson<RunTimeData>(text);
        fastestRunTime = data.fastestRunTime;
        Debug.Log("Loaded run time: " + fastestRunTime);
    }
    
    private void UGSSnitch()
    {
        RunTime runTime = new RunTime
        {
            RunTimeValue = lastRunTime
        };
        AnalyticsService.Instance.RecordEvent(runTime);
    }
}


// Unity Analytics event class
public class RunTime : Unity.Services.Analytics.Event
{
    public RunTime() : base("runTime")
    {
    }
    
    public float RunTimeValue { set { SetParameter("runTime", value); } }
}