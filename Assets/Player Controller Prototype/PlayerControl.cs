using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Input System
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;
    Rigidbody2D player;

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintRatio;

    public bool isSprinting;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Vector2 currentPosition = transform.position;
        float adjustedSpeed = moveSpeed;

        // Sprint functionality
        if (sprintAction.ReadValue<float>() > 0f)
        {
            adjustedSpeed *= sprintRatio;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        
        Vector2 vel = new Vector2(moveAction.ReadValue<Vector2>().x * adjustedSpeed, moveAction.ReadValue<Vector2>().y*adjustedSpeed);
        player.velocity = vel;
        
        // // Input System
        // float moveX = moveAction.ReadValue<Vector2>().x;
        // float moveY = moveAction.ReadValue<Vector2>().y;

        // currentPosition.x += moveX * adjustedSpeed * Time.deltaTime;
        // currentPosition.y += moveY * adjustedSpeed * Time.deltaTime;

        // // Setting up the boundary for the ball
        // currentPosition.x = Mathf.Clamp(currentPosition.x, -960, 960);
        // currentPosition.y = Mathf.Clamp(currentPosition.y, -540, 540);

        // Apply the new position every frame
        // transform.position = currentPosition;
    }
}