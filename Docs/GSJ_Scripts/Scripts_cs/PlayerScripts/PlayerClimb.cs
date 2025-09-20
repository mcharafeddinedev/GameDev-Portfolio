using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f;
    private bool isClimbing = false;
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource climbingSound; // Assign the climbing sound in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Check if climbingSound is assigned
        if (climbingSound == null)
        {
            Debug.LogError("Climbing sound AudioSource is not assigned!");
        }
    }

    void Update()
    {
        if (isClimbing)
        {
            float moveInputVertical = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, moveInputVertical * climbSpeed);

            // Set animation
            if (moveInputVertical != 0)
            {
                anim.SetBool("isClimbing", true);
                anim.SetBool("isIdle", false);

                // Play climbing sound
                if (climbingSound != null && !climbingSound.isPlaying)
                {
                    climbingSound.Play();
                }
            }
            else
            {
                anim.SetBool("isClimbing", false);
                // Stop the climbing sound when not climbing
                if (climbingSound != null && climbingSound.isPlaying)
                {
                    climbingSound.Stop();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1f;
            anim.SetBool("isClimbing", false);

            // Stop the climbing sound when exiting the ladder
            if (climbingSound != null && climbingSound.isPlaying)
            {
                climbingSound.Stop();
            }
        }
    }
}
