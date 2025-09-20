using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        SetCursorVisibility();
    }

    void SetCursorVisibility()
    {
        // Check if the current scene index is between 1 and 10
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        bool isVisible = currentSceneIndex >= 1 && currentSceneIndex <= 10 ? false : true;

        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCursorVisibility();
    }

    public void SetCursorVisible(bool visible, bool lockCursor)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
