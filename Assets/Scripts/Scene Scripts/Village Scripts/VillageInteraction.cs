using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VillageInteraction : MonoBehaviour
{
	[Header("Input")]
	Controls controls;
	[Tooltip("The initial delay (in seconds) between an initial move action and a repeated move action.")]
	[SerializeField] float moveRepeatDelay = 0.5f;
	[Tooltip("The speed (in seconds) that the move action repeats itself once repeating (max 1 per frame).")]
	[SerializeField] float moveRepeatRate = 0.1f;
	bool firstInput = true;
	float moveTimer = 0f;


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
		MovementInput();
	}

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

				if (moveTimer < 0f)
				{
					moveTimer = 0f;
				}
			}

			// Can move
			if (moveTimer <= 0f)
			{
				// Check if this is the first movement input after there was just no movement
				if (firstInput)
				{
					// Put move on moveRepeatDelay cooldown
					firstInput = false;
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
					navigationManager.moveInList(1);
				}
			}

		}

		// There's no horizontal movement input
		else
		{
			// Reset movement so that the next movement input is instant
			firstInput = true;
			moveTimer = 0f;
		}
	}

    void MovementInput()
    {
        // Need to check isPlaying so that these input events are not triggered before currentStory.currentChoices.Count is a valid reference
        // Check that there are dialogue options to choose from otherwise there's no option to move
        if (isPlaying && currentStory.currentChoices.Count > 0)
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

                    if (moveTimer < 0f)
                    {
                        moveTimer = 0f;
                    }
                }

                // Can Move
                if (moveTimer <= 0f)
                {
                    // Check if this is the first movement input after there was just no movement
                    if (firstInput)
                    {
                        // Put move on moveRepeatDelay cooldown
                        firstInput = false;
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
                        // Navigate up in the choices list
                        currentChoiceIndex--;
                        if (currentChoiceIndex < 0) currentChoiceIndex = currentStory.currentChoices.Count - 1;
                        // Optionally, call a function to update the UI here
                    }
                    else if (inputVector.x > 0f)        // right
                    {
                        // Navigate down in the choices list
                        currentChoiceIndex++;
                        if (currentChoiceIndex >= currentStory.currentChoices.Count) currentChoiceIndex = 0;
                        // Optionally, call a function to update the UI here
                    }
                }

            }

            // There's no horizontal movement input
            else
            {
                // Reset movement so that the next movement input is instant
                firstInput = true;
                moveTimer = 0f;
            }
        }
    }
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
