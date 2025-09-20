using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // added for IEnumerator
public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float flipDelay = 0.5f; // Delay in seconds before flipping again
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;
    private bool isBlocked = false;
    private bool canFlip = true;
    private float lastFlipTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void Update()
{
    // Check if there is ground or trap in front
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer | (1 << LayerMask.NameToLayer("Trap")));

    // Check if there is a wall in front
    isBlocked = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    // If blocked, turn around
    if ((isBlocked || !isGrounded) && canFlip)
    {
        Flip();
    }
}


    private void FixedUpdate()
    {
        // Move the slime
        Vector2 movement = new Vector2(moveSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void Flip()
    {
        canFlip = false;
        lastFlipTime = Time.time;
        moveSpeed = -moveSpeed; // Reverse movement direction
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void LateUpdate()
    {
        // Allow flipping again after a delay
        if (!canFlip && Time.time >= lastFlipTime + flipDelay)
        {
            canFlip = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Draw the detection circles in the Unity editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
    }
}