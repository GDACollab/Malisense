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

	// Rigid Body
	Rigidbody2D rb;

	// Stamina Script
	PlayerStamina playerStamina;

	// Movement
	[Header("Movement")]
	[SerializeField] float walkingSpeed;
	[SerializeField] float sprintRatio;
	[SerializeField] float sneakRatio;
	float adjustedSpeed;								// the speed the player moves at, accounting for sprinting or sneaking

	// Movement States
	[Header("Movement States (for other scripts to use)")]
	public bool isMoving;
	public bool isSprinting;
	public bool isSneaking;

	void Start()
	{
		// Set input system variables
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions.FindAction("8 Directions Movement");
		sprintAction = playerInput.actions.FindAction("Sprint");
		sneakAction = playerInput.actions.FindAction("Sneak");

		// Set rigid body variables
		rb = GetComponent<Rigidbody2D>();

		// Set stamina script variables
		playerStamina = GetComponent<PlayerStamina>();
	}

	private void FixedUpdate()
	// For moving and rotating
	{
		// Movement
		float moveX = moveAction.ReadValue<Vector2>().x;
		float moveY = moveAction.ReadValue<Vector2>().y;
		Vector2 direction = new Vector2(moveX, moveY).normalized;
		rb.velocity = direction * adjustedSpeed;

		// set isMoving
		if (moveX != 0 || moveY != 0)
			isMoving = true;
		else
			isMoving = false;

		// Rotation
		float angle = Vector2.SignedAngle(Vector2.right, direction) - 90;
		if (direction.magnitude > 0)
			rb.MoveRotation(angle);
	}

	void Update()
	// For sprinting and sneaking
	{
		// Walking (default)
		adjustedSpeed = walkingSpeed;

        // Sprinting
        if ((sprintAction.ReadValue<float>() > 0f) && (isMoving == true) && (!playerStamina.isExhausted))
		{
			adjustedSpeed *= sprintRatio;
			isSprinting = true;
		}
		else
		{
			isSprinting = false;
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
	}
}