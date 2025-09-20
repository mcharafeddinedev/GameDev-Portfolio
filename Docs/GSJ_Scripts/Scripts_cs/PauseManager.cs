using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public InputActionReference pauseAction;

    public GameObject pauseMenuUI; // Reference to the Pause Menu UI GameObject

    void Awake()
    {
        // Instantiate the Pause Menu UI if it's assigned
        if (GameManager.Instance != null && GameManager.Instance.pauseMenuUI != null)
        {
            pauseMenuUI = GameManager.Instance.pauseMenuUI;
            pauseMenuUI.SetActive(false); // Ensure it's initially inactive
            DontDestroyOnLoad(pauseMenuUI); // Make sure it persists throughout scenes
        }
        else
        {
            Debug.LogError("Pause Menu UI is not assigned in GameManager!");
        }
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += PauseGame;
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= PauseGame;
            pauseAction.action.Disable();
        }
    }

    void PauseGame(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        GameManager.Instance.ResumeGame();
        isPaused = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        GameManager.Instance.PauseGame();
        isPaused = true;
    }
}
