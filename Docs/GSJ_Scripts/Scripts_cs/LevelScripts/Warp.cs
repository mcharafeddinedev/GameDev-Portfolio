using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    public AudioSource warpSound; // AudioSource for warp sound
    public float delayBeforeSceneChange = 0.5f; // Delay in seconds before scene change
    private bool hasPlayedSound = false; // Flag to track if warp sound has been played

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false; // Disable the PlayerController
            }

            if (!hasPlayedSound)
            {
                StopAllOtherSounds();
                GoToNextSceneWithDelay();
            }
        }
    }

    private void StopAllOtherSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != warpSound)
            {
                audioSource.Stop();
            }
        }
    }

    private void GoToNextSceneWithDelay()
    {
        PlayWarpSound(); // Play warp sound before loading next scene
        Invoke("GoToNextScene", delayBeforeSceneChange);
    }

    private void GoToNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ReloadCurrentScene();
        }
    }

    private void ReloadCurrentScene()
    {
        PlayWarpSound(); // Play warp sound before reloading current scene
        Invoke("ReloadScene", delayBeforeSceneChange);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlayWarpSound()
    {
        if (warpSound != null && !hasPlayedSound)
        {
            warpSound.Play();
            hasPlayedSound = true;
        }
        else
        {
            Debug.LogWarning("Warp sound is not assigned or has already been played!");
        }
    }
}
