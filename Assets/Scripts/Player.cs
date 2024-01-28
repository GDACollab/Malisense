using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10;   // Movement Speed Multiplier
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private Rigidbody2D rb;
    private Vector2 inputVector = new Vector2();
    private bool facingRight = true;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // If the input is moving the player right and the player is facing left
        if (inputVector.x > 0 && !facingRight)
            Flip();
        // If the input is moving the player left and the player is facing right
        else if (inputVector.x < 0 && facingRight)
            Flip();
    }

    void FixedUpdate()
    {
        // Move player
        bool success = MovePlayer(inputVector);

        // Determine which axis that player cannot move
        if (!success)
        {
            // Try Left / Right
            success = MovePlayer(new Vector2(inputVector.x, 0));

            if (!success)
            {
                success = MovePlayer(new Vector2(0, inputVector.y));
            }
        }
    }

    public bool MovePlayer(Vector2 direction)
    {
        // Check for potential collisions
        int count = rb.Cast(
            direction,      // Represent the direction from the body to look for
            movementFilter, // The settings that determine where ac ollision can occur on such layers
            castCollisions, // List of collisions to store the found collisions into after Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset // The distance to cast equal to next frame moved
        );
        // No collisions
        if (count == 0)
        {
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            // Move character
            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            // Print collisions
            foreach (RaycastHit2D hit in castCollisions)
            {
                print(hit.ToString());
            }

            return false;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}