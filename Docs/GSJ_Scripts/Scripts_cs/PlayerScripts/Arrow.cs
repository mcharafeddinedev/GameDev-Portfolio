using UnityEngine;

public class Arrow : MonoBehaviour
{
    public AudioSource arrowReleaseSound;   // Sound when the arrow is released

    public float time = 3f; // Lifetime of the arrow
    public float speed = 4f; // Horizontal speed of the arrow
    Rigidbody2D rb; // Rigidbody2D component of the arrow

    bool hasHitSomething = false; // Flag to track if the arrow has hit something

    void Start()
    {
        // Find and assign the AudioSource for the release sound
        arrowReleaseSound = GetComponent<AudioSource>();

        // Check if AudioSource was found
        if (arrowReleaseSound == null)
        {
            Debug.LogError("AudioSource not found on the arrow GameObject!");
        }

        Invoke("DestroyArrow", time); // Invoke DestroyArrow with a delay
        rb = GetComponent<Rigidbody2D>();

        // Determine the initial direction based on player's facing direction
        Vector2 shootDirection = transform.right;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get the local scale of the player
            Vector3 playerScale = player.transform.localScale;

            // Determine shoot direction based on player scale
            if (playerScale.x < 0)
            {
                // Player is facing left, flip the shoot direction
                shootDirection = -shootDirection;
            }
        }

        // Set initial velocity for arrow
        rb.velocity = shootDirection * speed;

        // Play arrow release sound immediately when the arrow is shot
        if (arrowReleaseSound != null)
        {
            arrowReleaseSound.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHitSomething)
            return; // If already hit something, ignore further collisions
        
        if (collision.CompareTag("Ground"))
        {
            DestroyArrow();
        }
        else if (collision.CompareTag("Enemy"))
        {
            HitEnemy(collision.gameObject);
        }
    }

    void HitEnemy(GameObject enemy)
{
    Debug.Log("Loading audio clip...");

    hasHitSomething = true; // Set flag to true to prevent further collisions

    // Play arrow hit enemy sound
    AudioSource arrowHitEnemySound = gameObject.AddComponent<AudioSource>();
    arrowHitEnemySound.clip = Resources.Load<AudioClip>("arrowHit");
    if (arrowHitEnemySound.clip != null)
    {
        arrowHitEnemySound.Play();
        Debug.Log("Audio clip loaded and played successfully!");
    }
    else
    {
        Debug.LogError("Failed to load audio clip!");
    }

    Destroy(enemy);

    // Check if the enemy has a Collider2D component
    Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
    if (enemyCollider != null)
    {
        Destroy(enemyCollider);
    }

    // Destroy the arrow with a delay to allow sound to play out
    DestroyArrowWithDelay();
}

    void DestroyArrowWithDelay()
    {
        // Cancel any previous Invoke calls to DestroyArrow
        CancelInvoke("DestroyArrow");
        
        // Invoke DestroyArrow with a delay
        Invoke("DestroyArrow", .6f); // Adjust the delay as needed
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
