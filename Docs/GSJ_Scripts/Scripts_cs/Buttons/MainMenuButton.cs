using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenuButton : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName); // Load the main menu scene
    }
}
