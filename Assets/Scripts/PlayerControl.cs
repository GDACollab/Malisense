using System;
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
    InputAction sneakAction;

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintRatio;
    [SerializeField] float sneakRatio;

    public bool isSprinting;
    public bool isSneaking;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
        sneakAction = playerInput.actions.FindAction("Sneak");
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;
        float adjustedSpeed = moveSpeed;

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
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;

        currentPosition.x += moveX * adjustedSpeed * Time.deltaTime;
        currentPosition.y += moveY * adjustedSpeed * Time.deltaTime;

        // Setting up the boundary for the ball
        currentPosition.x = Mathf.Clamp(currentPosition.x, -960, 960);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -540, 540);
        
        // Apply the new position every frame
        transform.position = currentPosition;
        // Rotate every frame, change to snap to 45degree increments
        transform.up =currentPosition;
    }
}