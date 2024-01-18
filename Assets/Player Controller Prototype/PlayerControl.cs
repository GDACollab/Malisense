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

    [SerializeField] float moveSpeed;
    [SerializeField] float sprintSpeed;
    float diagonalRatio = (float)Math.Sqrt(1.0 / 2.0);

    private NewControls newControls;
    public bool isSprinting;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
    }

    private void Awake()
    {
        newControls = new NewControls();

        newControls.Player.StartSprint.performed += x => SprintPressed();
        newControls.Player.EndSprint.performed += x => SprintReleased();
    }
    private void SprintPressed()
    {
        isSprinting = true;
    }
    private void SprintReleased()
    {
        isSprinting = false;
    }
    void Update()
    {
        Vector2 currentPosition = transform.position;
        float adjustedSpeed = moveSpeed;
        float adjustedSprintSpeed = sprintSpeed;

        // If moving diagonally, decrease the speed based on the Pythagorean Theorem
        if ((moveAction.ReadValue<Vector2>().y > 0 && (moveAction.ReadValue<Vector2>().x < 0 || moveAction.ReadValue<Vector2>().x > 0)) ||
            (moveAction.ReadValue<Vector2>().y < 0 && (moveAction.ReadValue<Vector2>().x < 0 || moveAction.ReadValue<Vector2>().x > 0)))
        {
            adjustedSpeed *= diagonalRatio;
            adjustedSprintSpeed *= diagonalRatio;
        }

        // Input System
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;

        if (isSprinting)
        {
            currentPosition.x += moveX * adjustedSprintSpeed * Time.deltaTime;
            currentPosition.y += moveY * adjustedSprintSpeed * Time.deltaTime;
        }
        else
        {
            currentPosition.x += moveX * adjustedSpeed * Time.deltaTime;
            currentPosition.y += moveY * adjustedSpeed * Time.deltaTime;
        }
        

        // Setting up the boundary for the ball
        currentPosition.x = Mathf.Clamp(currentPosition.x, -960, 960);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -540, 540);

        // Apply the new position every frame
        transform.position = currentPosition;
    }

    private void OnEnable()
    {
        newControls.Enable();
    }

    private void OnDisable()
    {
        newControls.Disable();
    }
}
