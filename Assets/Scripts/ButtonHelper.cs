

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHelper : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("PCGTestScene"); // Start new round
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene"); // Return to main menu
    }
}

