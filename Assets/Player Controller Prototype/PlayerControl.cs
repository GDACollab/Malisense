using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using System; // Math.Sqrt();

public class PlayerControl : MonoBehaviour
{
    // Input System
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintRatio;

    public bool isSprinting;
    float diagonalRatio = (float)Math.Sqrt(1.0 / 2.0);

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;
        float adjustedSpeed = moveSpeed;

        // If moving diagonally, decrease the speed based on the Pythagorean Theorem
        if ((moveAction.ReadValue<Vector2>().y > 0 && (moveAction.ReadValue<Vector2>().x < 0 || moveAction.ReadValue<Vector2>().x > 0)) ||
            (moveAction.ReadValue<Vector2>().y < 0 && (moveAction.ReadValue<Vector2>().x < 0 || moveAction.ReadValue<Vector2>().x > 0)))
        {
            adjustedSpeed *= diagonalRatio;
        }

        if (sprintAction.ReadValue<float>() > 0f)
        {
            adjustedSpeed *= sprintRatio;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        // Input System
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;

        currentPosition.x += moveX * adjustedSpeed * Time.deltaTime;
        currentPosition.y += moveY * adjustedSpeed * Time.deltaTime;

        // Setting up the boundary for the ball
        currentPosition.x = Mathf.Clamp(currentPosition.x, -960, 960);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -540, 540);

        // Apply the new position every frame
        transform.position = currentPosition;
    }
}