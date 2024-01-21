using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Input System
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;


    [SerializeField] float moveSpeed;
    [SerializeField] float sprintRatio;
    [SerializeField] float maxStamina; //How many seconds of sprinting
    [SerializeField] float minimumToSprint; //Determines when player can run again, the variable should be a percentage
    float currentStamina; //How many seconds of springing left
    bool exhausted = false; //If character's stamina reaches below 0, give exhausted state to player which makes it so they cannot run 
    // until stamina regenerates back to a certain threshold.

    public bool isSprinting;

    void Start()
    {

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("8 Directions Movement");
        sprintAction = playerInput.actions.FindAction("Sprint");
        currentStamina = maxStamina; 

    }

    void Update()
    {
        Vector2 currentPosition = transform.position;
        float adjustedSpeed = moveSpeed;

        //Ensures that currentStamina doesn't go over max Stamina
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;

        //Makes character not exhausted anymore
        if (currentStamina > maxStamina * minimumToSprint)
            exhausted = false;

        // Sprint functionality
        if ((sprintAction.ReadValue<float>() > 0f) && (currentStamina > 0f) && (!exhausted)) //Two new test cases
            //2nd check to see if currentStamina greater than 0, then check to see if character is exhausted
        {
            currentStamina -= 1 * Time.deltaTime; //The rate at which stamina drains (drains 1 every second)
            if (currentStamina < 0f) //Makes the exhaust variable true, which means the character cannot run for some time.
                exhausted = true;

            adjustedSpeed *= sprintRatio;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;

            if (currentStamina < maxStamina)
                currentStamina += 1 * Time.deltaTime; //The rate at which stamina regenerates (Regenerates 1 every second)

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