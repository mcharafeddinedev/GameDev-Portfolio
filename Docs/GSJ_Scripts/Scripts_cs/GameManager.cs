using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool ironManMode = false;
    private int currentLevel = 1; // Add a variable to track the current level/stage
    public GameObject levelTextGO; // Reference to the GameObject for displaying the level
    public GameObject pauseMenuPrefab; // Reference to the Pause Menu UI prefab

    public CursorManager CursorManager { get; private set; } // Reference to the CursorManager

    public GameObject pauseMenuUI; // Reference to the Pause Menu UI GameObject

    private TextMesh levelText; // Reference to the TextMesh component for displaying the level

    void Awake()
    {
        // Ensure GameManager is a singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CursorManager = GetComponent<CursorManager>(); // Get CursorManager component

        // Load the pause menu UI prefab if it's assigned
        if (pauseMenuPrefab != null)
        {
            pauseMenuUI = Instantiate(pauseMenuPrefab);
            pauseMenuUI.SetActive(false); // Ensure it's initially inactive
            DontDestroyOnLoad(pauseMenuUI); // Make sure it persists throughout scenes
        }
        else
        {
            Debug.LogError("Pause Menu Prefab is not assigned in GameManager!");
        }

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure the game is unpaused and timescale is set to 1
        Time.timeScale = 1f;
        GameManager.Instance.ResumeGame(); // Make sure the game is resumed

        // Update the level text whenever a new scene is loaded
        UpdateLevelText();

        // Ensure the Pause Menu UI is inactive on scene load
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Pause Menu UI is not assigned in GameManager!");
        }

        // Ensure the cursor is locked and invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Check if the current scene index is between 1 and 10
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex >= 1 && currentSceneIndex <= 10)
        {
            CursorManager.SetCursorVisible(false, true); // Hide cursor and lock it
        }
        else
        {
            CursorManager.SetCursorVisible(true, false); // Show cursor and unlock it
        }
    }

    public void UpdateLevelText()
    {
        if (levelTextGO != null)
        {
            // Get the TextMesh component from the levelTextGO
            levelText = levelTextGO.GetComponent<TextMesh>();
            if (levelText != null)
            {
                // Display the current scene index as the level number
                levelText.text = "LVL " + SceneManager.GetActiveScene().buildIndex.ToString();
            }
            else
            {
                Debug.LogWarning("TextMesh component not found in Level Text GameObject!");
            }
        }
        else
        {
            Debug.LogWarning("Level Text GameObject is not assigned!");
        }
    }

    public void SetIronManMode(bool enabled)
    {
        ironManMode = enabled;
    }

    public bool IsIronManModeEnabled()
    {
        return ironManMode;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
        UpdateLevelText(); // Update level text when the current level changes
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            currentLevel++; // Increment current level when loading next scene
        }
        else
        {
            ReloadCurrentScene();
        }
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);

            // Set cursor to visible and unlock state to interact with UI
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.LogError("Pause Menu UI is not assigned in GameManager!");
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);

            // Set cursor to invisible and lock state
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Debug.LogError("Pause Menu UI is not assigned in GameManager!");
        }

        // Check if the current scene index is between 1 and 10
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex >= 1 && currentSceneIndex <= 10)
        {
            CursorManager.SetCursorVisible(false, true); // Hide cursor and lock it
        }
        else
        {
            CursorManager.SetCursorVisible(true, false); // Show cursor and unlock it
        }
    }
}
