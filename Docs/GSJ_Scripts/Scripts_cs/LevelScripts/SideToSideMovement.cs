using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    public float speed = 2.0f; // Speed of the platform
    public float distance = 5.0f; // Distance the platform should move

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private bool movingRight = true;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector2.right * distance;
        Rigidbody2D platformRb = GetComponent<Rigidbody2D>();
        platformRb.bodyType = RigidbodyType2D.Dynamic; // Ensure the Rigidbody is dynamic
    }

    void Update()
    {
        if (movingRight)
        {
            // Move towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if we've reached the target position
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingRight = false;
            }
        }
        else
        {
            // Move towards the initial position
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);

            // Check if we've reached the initial position
            if (Vector2.Distance(transform.position, initialPosition) < 0.01f)
            {
                movingRight = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the collision is happening from above
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            int contactCount = collision.GetContacts(contacts);
            for (int i = 0; i < contactCount; i++)
            {
                if (Vector2.Dot(contacts[i].normal, Vector2.up) > 0.5f)
                {
                    // The collision is from above, treat the platform as ground
                    Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Re-enable collision between player and platform
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false);
        }
    }
}
