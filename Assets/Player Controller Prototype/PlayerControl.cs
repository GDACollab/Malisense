using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
	// Input System
	PlayerInput playerInput;
	InputAction moveAction;
	InputAction sprintAction;
	InputAction sneakAction;

    // Rigid Body
    Rigidbody2D rb;

    // Movement
    [Header("Movement")]
	[SerializeField] float moveSpeed;
	[SerializeField] float sprintRatio;
	[SerializeField] float sneakRatio;
	[SerializeField] float maxStamina;
    float adjustedSpeed;

    // Movement States
    [Header("Movement States (for other scripts to use)")]
    public bool isMoving;
    public bool isSprinting;
    public bool isSneaking;

    // Stamina
    // these need to be moved to the stamina script later
    [Tooltip("percentage of stamina required to sprint again")] [Range(0.00f, 1.00f)] [SerializeField] float minimumToSprint;
    [Tooltip("n stamina per second")] [SerializeField] float staminaRegen;
    [Tooltip("n stamina per second")] [SerializeField] float staminaDepletion;
    float currentStamina;
	bool exhausted = false;                 // makes it so player can't run; true when stamina is 0, false when currentStamina >= minimumToSprint
    
	// Stamina Bar
	// these need to be moved to the stamina script later
    [Header("UI")]
    public Image StaminaBar;

	void Start()
	{
		// Set Input System Variables
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions.FindAction("8 Directions Movement");
		sprintAction = playerInput.actions.FindAction("Sprint");
		sneakAction = playerInput.actions.FindAction("Sneak");

		// Set Rigid Body Variables
		rb = GetComponent<Rigidbody2D>();

        // Set Initial Stamina
        // these need to be moved to the stamina script later
        currentStamina = maxStamina;
	}

    private void FixedUpdate()		// for moving and rotating
    {
		// Movement
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;
        Vector2 direction = new Vector2(moveX, moveY).normalized;
        rb.velocity = direction * adjustedSpeed;

        // set isMoving
        if (moveX != 0 || moveY != 0) isMoving = true;
        else isMoving = false;

        // Rotation
        float angle = Vector2.SignedAngle(Vector2.right, direction) - 90;
        if (direction.magnitude > 0)
        {
            rb.MoveRotation(angle);
        }
    }

    void Update()		// for sprinting and sneaking
	{
		adjustedSpeed = moveSpeed;

        //Ensures that currentStamina doesn't go over max Stamina
        // these need to be moved to the stamina script later
        if (currentStamina > maxStamina)
			currentStamina = maxStamina;

        //Makes character not exhausted anymore
        // these need to be moved to the stamina script later
        if (currentStamina > maxStamina * minimumToSprint)
			exhausted = false;

		// Sprinting
		if ((sprintAction.ReadValue<float>() > 0f) && (currentStamina > 0f) && (!exhausted) && (isMoving == true)) //Two new test cases (actually added a 3rd, only starts sprinting if moving)
																							 //2nd check to see if currentStamina greater than 0, then check to see if character is exhausted
		{
			currentStamina -= staminaDepletion * Time.deltaTime; //The rate at which stamina drains (drains 1 every second)
			if (currentStamina < 0f) //Makes the exhaust variable true, which means the character cannot run for some time.
				exhausted = true;
			
			adjustedSpeed *= sprintRatio;
			isSprinting = true;
		}
		else
		{
			isSprinting = false;

			if (currentStamina < maxStamina)
				currentStamina += staminaRegen * Time.deltaTime; //The rate at which stamina regenerates (Regenerates 1 every second)

		}

		// Sneaking
		if (sneakAction.ReadValue<float>() > 0f && !isSprinting)
		{
			adjustedSpeed *= sneakRatio;
			isSneaking = true;
		}
		else
		{
			isSneaking = false;
		}

        //stamina bar stuff, just temporary janky bar to keep track of stamina easier for debug purposes, can change it and make it look nicer later
        // these need to be moved to the stamina script later
        StaminaBar.fillAmount = currentStamina / maxStamina;
	}
}