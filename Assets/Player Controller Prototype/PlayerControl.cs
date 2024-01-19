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

	// Movement
	[SerializeField] float moveSpeed;
	[SerializeField] float sprintRatio;
	public bool isSprinting;

	void Start()
	{
		// Populate the Input System variables
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions.FindAction("8 Directions Movement");
		sprintAction = playerInput.actions.FindAction("Sprint");
	}

	void Update()
	{
		Vector2 newPosition = transform.position;		// where the player should move to
		float adjustedSpeed = moveSpeed;				// the distance the player moves, adjusted for sprinting

		// Player is sprinting?
		if (sprintAction.ReadValue<float>() > 0f)
		{
			adjustedSpeed *= sprintRatio;
			isSprinting = true;
		}
		else
		{
			isSprinting = false;
		}

		// Calculate the position the player will move to
		newPosition += moveAction.ReadValue<Vector2>() * adjustedSpeed * Time.deltaTime;

		// Don't let player move past the boundary
		newPosition.x = Mathf.Clamp(newPosition.x, -960, 960);
		newPosition.y = Mathf.Clamp(newPosition.y, -540, 540);

		// Apply the new position every frame
		transform.position = newPosition;
	}
}