using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int coinValue = 1;
    private ScoreManager scoreManager;
    private bool isCollected = false;
    public AudioSource collectSound; // AudioSource for collect sound

    void Start()
    {
        scoreManager = ScoreManager.Instance; // Access the ScoreManager singleton
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in scene!");
        }

        // Get the AudioSource component attached to this GameObject
        collectSound = GetComponent<AudioSource>();

        // Check if collectSound is assigned
        if (collectSound == null)
        {
            Debug.LogError("AudioSource for collect sound is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && collision.gameObject.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
    void CollectCoin()
{
    isCollected = true; // Mark the coin as collected to prevent multiple collections

    if (scoreManager != null)
    {
        scoreManager.AddScore(coinValue);
    }

    // Play the collect sound using PlayOneShot
    if (collectSound != null)
    {
        Debug.Log("Playing collect sound");
        collectSound.PlayOneShot(collectSound.clip);
    }
    else
    {
        Debug.LogWarning("Collect sound is not assigned!");
    }

    // Destroy the GameObject after a short delay
    Destroy(gameObject, 0.25f);
}

}