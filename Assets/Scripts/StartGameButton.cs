using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    
    private void Start()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        }
        else
        {
            Debug.LogError("Start Game Button is not assigned in the inspector.");
        }
    }
    
    private void OnStartGameButtonClicked()
    {
        GameManager.Instance.StartGame(); // Start the game
    }
}