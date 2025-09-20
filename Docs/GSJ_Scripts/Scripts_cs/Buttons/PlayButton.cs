using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button playButton;

    void Start()
    {
        // Get the Button component from the GameObject
        playButton = GetComponent<Button>();

        // Add onClick listener to the button
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on PlayButton GameObject!");
        }
    }

    void OnPlayButtonClick()
    {
        // Load the game scene when the button is clicked
        SceneManager.LoadScene("Platformer0");
    }
}
