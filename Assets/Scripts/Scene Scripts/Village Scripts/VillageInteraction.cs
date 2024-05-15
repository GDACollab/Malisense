using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// An input-handling script that calls functions on the Village Navigation and Dialogue Managers.
/// </summary>
public class VillageInteraction : MonoBehaviour
{
	[Header("Input")]
	Controls controls;
	[Tooltip("The initial delay (in seconds) between an initial move action and a repeated move action.")]
	[SerializeField] float moveRepeatDelay = 0.5f;
	[Tooltip("The speed (in seconds) that the move action repeats itself once repeating (max 1 per frame).")]
	[SerializeField] float moveRepeatRate = 0.1f;
	private bool isFirstInput = true;
    private float moveTimer = 0f;


	[Header("Manager Scripts")]
	[SerializeField] private VillageNavigationManager navigationManager;
    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
	{
        // Input
        controls = new Controls();
		controls.UI.Enable();
		controls.UI.Select.performed += Select;
	}


	private void Update()
	{
		// Move selection based on directional input
		MovementInput();
	}

    /// <summary>
    ///  Checks for input to move left or right, and triggers the village navigation or dialogue UI to reflect the movement.
    /// </summary>
    void MovementInput()
	{
		// Read movement input
		Vector2 inputVector = controls.UI.Move.ReadValue<Vector2>();

		// There's horizontal movement input
		if (inputVector.x != 0f)
		{
			// Moving is on cooldown
			if (moveTimer > 0f)
			{
				moveTimer -= Time.deltaTime;

				if (moveTimer < 0f) { 
					moveTimer = 0f; 
				}
			}

			// Can move
			if (moveTimer <= 0f)
			{
				// Check if this is the first movement input after there was just no movement
				if (isFirstInput)
				{
					// Put move on moveRepeatDelay cooldown
					isFirstInput = false;
					moveTimer = moveRepeatDelay;
				}
				else
				{
					// Put move on moveRepeatRate cooldown
					moveTimer = moveRepeatRate;
				}

				// Move
				if (inputVector.x < 0f)             // left
				{
                    if (navigationManager.hasEntered)
                    {
                        dialogueManager.moveChoiceSelection("left");
                    }
                    else
                    {
                        navigationManager.moveInList(-1);
                    }
				}
				else if (inputVector.x > 0f)        // right
				{
                    if (navigationManager.hasEntered)
                    {
                        dialogueManager.moveChoiceSelection("right");
                    }
                    else
                    {
                        navigationManager.moveInList(1);
                    }
                }
			}

		}

		// There's no horizontal movement input
		else
		{
			// Reset movement so that the next movement input is instant
			isFirstInput = true;
			moveTimer = 0f;
		}
	}

    /// <summary>
    ///  Is called on the input to select, and triggers the village navigation or dialogue UI to reflect the selection.
    /// </summary>
    void Select(InputAction.CallbackContext context)
	{
		if (navigationManager.hasEntered)
		{
            dialogueManager.selectDialogue();
        } else
		{
            navigationManager.selectBuilding();
		}
	}
}
