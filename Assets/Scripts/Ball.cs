using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballSpeed = 5f;
    private Vector2 ballDirection;
    private Rigidbody2D rb;

    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Start the ball movement with a random direction
        ballDirection = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        rb.velocity = ballDirection * ballSpeed;
    }

    void Update()
    {
        // Keep the ball's speed constant
        rb.velocity = rb.velocity.normalized * ballSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Reverse the ball's x or y direction depending on the wall
            if (collision.contacts[0].normal.x != 0) // Side walls
            {
                ballDirection.x = -ballDirection.x;
            }
            else if (collision.contacts[0].normal.y != 0) // Top or bottom walls
            {
                ballDirection.y = -ballDirection.y;
            }

            // Apply the new velocity
            rb.velocity = ballDirection * ballSpeed;
        }

        // Handle collision with paddle
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Reflect the ball off the paddle based on where it hits
            float hitFactor = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            ballDirection = new Vector2(hitFactor, 1).normalized;

            // Apply the new velocity
            rb.velocity = ballDirection * ballSpeed;
        }

        // Handle collision with blocks
        if (collision.gameObject.CompareTag("Brick"))
        {
            // Destroy the block
            //Destroy(collision.gameObject);

            // Reflect the ball
            ballDirection.y = -ballDirection.y;

            // Apply the new velocity
            rb.velocity = ballDirection * ballSpeed;
        }
    }
}
