using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Input System
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;
    InputAction sneakAction;

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintRatio;
    [SerializeField] float sneakRatio;

    public bool isSprinting;
    public bool isSneaking;

    public Rigidbody2D rb;

    float adjustedSpeed;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
        sneakAction = playerInput.actions.FindAction("Sneak");
        
    }

    private void FixedUpdate()
    {
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;
        
        Vector2 direction = new Vector2(moveX, moveY).normalized;

        rb.velocity = direction * adjustedSpeed;
        float angle = Vector2.SignedAngle(Vector2.right, direction) - 90;

        if (direction.magnitude > 0)
        {
            rb.MoveRotation(angle);   
        }
    }

    void Update()
    {
        adjustedSpeed = moveSpeed;
        
        Vector2 currentPosition = transform.position;

        // Sprint functionality
        if (sprintAction.ReadValue<float>() > 0f && !isSneaking)
        {
            adjustedSpeed *= sprintRatio;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        
        if (sneakAction.ReadValue<float>() > 0f && !isSprinting)
        {
            adjustedSpeed *= sneakRatio;
            isSneaking = true;
        }
        else
        {
            isSneaking = false;
        }

        // Input System
        

        // Setting up the boundary for the ball
        currentPosition.x = Mathf.Clamp(currentPosition.x, -960, 960);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -540, 540);
        
        // Apply the new position every frame
        // Rotate every frame, change to snap to 45degree increments
        //transform.up =currentPosition;
    }
}