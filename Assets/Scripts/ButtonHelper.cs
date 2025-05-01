

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHelper : MonoBehaviour
{
    public void LoadGameScene()
    {
        GameManager.Instance.StartGame(); // Start the game
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene"); // Return to main menu
    }
}

