using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput; // Stores the movement input from the player
    Rigidbody2D rb; // Rigidbody2D component of the player
    Animator anim; // Animator component for controlling animations
    bool isGrounded; // Flag to check if the player is grounded
    bool canMove = true; // Flag to control player movement
    public bool isAlive = true; // Flag to track player's alive status
    public float runSpeed = 6; // Speed of player's horizontal movement
    public float jumpSpeed = 5; // Speed of player's jump
    public GameObject Arrow; // Reference to the arrow prefab
    [SerializeField] AudioSource sfx; // AudioSource for playing sounds
    [SerializeField] AudioClip jumpSound; // Jump sound effect
    [SerializeField] AudioClip deathSound; // Death sound effect

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>(); // Get the Animator component
        sfx = GetComponent<AudioSource>(); // Get the AudioSource component

        // Check if Iron Man mode is enabled (if GameManager exists and Iron Man mode is enabled)
        if (GameManager.Instance != null && GameManager.Instance.IsIronManModeEnabled())
        {
            // Additional setup for Iron Man mode if needed
        }
    }

    private void Update()
    {
        // Update player movement only if alive and allowed to move
        if (isAlive && canMove)
        {
            Run(); // Move the player
        }
    }

    // Handle movement input from the player
    private void OnMove(InputValue value)
    {
        if (!isAlive || !canMove)
            return;

        moveInput = value.Get<Vector2>(); // Get movement input from the player

        // Flip player sprite based on movement direction
        if (moveInput.x < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    // Handle jump input from the player
    private void OnJump(InputValue value)
    {
        if (!isAlive || !canMove)
            return;

        // Perform jump action if player is grounded and jump input is pressed
        if (value.isPressed && isGrounded)
        {
            rb.velocity += new Vector2(0f, jumpSpeed);
            anim.SetBool("isJumping", true); // Trigger jump animation
            if (jumpSound != null)
            {
                sfx.PlayOneShot(jumpSound); // Play jump sound effect
            }
            isGrounded = false; // Update grounded status
        }
    }

    // Handle shoot input from the player
    private void OnShoot(InputValue value)
    {
        if (!isAlive || !canMove)
            return;

        // Start shooting action when shoot input is pressed
        if (value.isPressed)
        {
            anim.SetBool("isShoot", true); // Trigger shoot animation
            isGrounded = true; // Set isGrounded to true to allow shooting while jumping
            isAlive = true; // Ensure player is marked as alive
        }
        else
        {
            anim.SetBool("isShoot", false); // Ensure shoot animation is disabled
        }
    }

    // Perform shooting action
    public void OnFire()
    {
        if (!isAlive || !canMove)
            return;

        // Instantiate and configure the arrow object
        GameObject tmp = Instantiate(Arrow);
        tmp.SetActive(true);
        tmp.transform.parent = Arrow.transform.parent;
        tmp.transform.position = Arrow.transform.position;
        tmp.transform.parent = null;
        tmp.transform.localScale = new Vector2(transform.localScale.x, tmp.transform.localScale.y);
    }

    // Move the player horizontally
    private void Run()
    {
        Vector2 velo = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = velo;

        // Update animation parameters based on player's movement
        anim.SetBool("isIdle", rb.velocity.x == 0);
        anim.SetBool("isRunning", rb.velocity.x != 0);

        // Check if the player is grounded
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;
        anim.SetBool("isJumping", !isGrounded); // Update jump animation based on grounded status
    }

    // Handle collisions with other objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collides with an enemy and is still alive
        if (other.CompareTag("Enemy") && isAlive)
        {
            Die(); // Handle player death
        }
    }

    // Reload the current scene
    private void ReloadScene()
    {
        GameManager.Instance.ReloadCurrentScene();
    }

    // Handle player death
    public void Die()
    {
        isAlive = false; // Mark player as dead
        anim.SetBool("isAlive", isAlive); // Update animation parameter for death
        if (deathSound != null && sfx != null)
        {
            sfx.PlayOneShot(deathSound); // Play death sound effect
        }

        // Reset the score when the player dies
        ScoreManager.Instance.ResetScore();

        // Update the level text when the player dies
        GameManager.Instance.UpdateLevelText();

        canMove = false; // Disable player movement upon death

        // Reload the level after a delay
        if (GameManager.Instance != null && GameManager.Instance.IsIronManModeEnabled())
        {
            Invoke("ReloadFirstLevel", 3f);
        }
        else
        {
            Invoke("ReloadScene", 3.5f);
        }
    }

    // Reload the first level of the game
    private void ReloadFirstLevel()
    {
        SceneManager.LoadScene("Platformer0");
    }
}

