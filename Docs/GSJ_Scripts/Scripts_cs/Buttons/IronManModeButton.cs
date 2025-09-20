using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IronManModeButton : MonoBehaviour
{
    private Button ironManModeButton;

    void Start()
    {
        // Get the Button component from the GameObject
        ironManModeButton = GetComponent<Button>();

        // Add onClick listener to the button
        if (ironManModeButton != null)
        {
            ironManModeButton.onClick.AddListener(OnIronManModeButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on IronManModeButton GameObject!");
        }
    }

    void OnIronManModeButtonClick()
    {
        // Set Iron Man mode to true
        GameManager.Instance.SetIronManMode(true);

        // Load the game scene when the button is clicked
        SceneManager.LoadScene("Platformer0");
    }
}
