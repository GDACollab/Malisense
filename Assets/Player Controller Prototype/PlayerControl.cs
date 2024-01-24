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

	// Movement
	[Header("Movement")]
	[SerializeField] float moveSpeed;
	[SerializeField] float sprintRatio;
	[SerializeField] float sneakRatio;
	float adjustedSpeed;

	// Movement States (for other scripts to use)
	[Header("Movement States")]
	public bool isMoving;
	public bool isSprinting;
	public bool isSneaking;

	// Player Stamina Script (to know when player is exhausted)
	[Header("Player Stamina Script")]
	[SerializeField] PlayerStamina playerStamina;


	void Start()
	{
		// Set Input System Variables
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions.FindAction("8 Directions Movement");
		sprintAction = playerInput.actions.FindAction("Sprint");
		sneakAction = playerInput.actions.FindAction("Sneak");

		// Set Rigid Body Variables
		rb = GetComponent<Rigidbody2D>();
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
		adjustedSpeed = moveSpeed;

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