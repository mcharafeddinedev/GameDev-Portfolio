using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    // Method to handle button click event
    public void OpenSettingsMenu()
    {
        // Load the settings menu scene or activate the settings menu UI panel
        SceneManager.LoadScene("PlatformerSettingsMenu");
    }
}
