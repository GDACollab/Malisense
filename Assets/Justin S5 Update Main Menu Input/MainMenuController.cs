using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{


	public void Move(InputAction.CallbackContext context)
	{
		// when you press a button, there are 3 phases that happen: started, performed, and canceled
		// we don't want this function to run 3 times when the select button is pressed, so we check for the performed stage
		if (context.performed)
		{
			Vector2 inputVector = context.ReadValue<Vector2>();

			if (inputVector.y > 0f)
			{
				// up
				Debug.Log("up");
			}
			else if (inputVector.y < 0f)
			{
				// down
				Debug.Log("down");
			}
			else if (inputVector.x > 0f)
			{
				// if slider, right
				Debug.Log("right");
			}
			else if (inputVector.x < 0f)
			{
				// if slider, left
				Debug.Log("left");
			}
		}
	}

	public void Select(InputAction.CallbackContext context)
	{
		// when you press a button, there are 3 phases that happen: started, performed, and canceled
		// we don't want this function to run 3 times when the select button is pressed, so we check for the performed stage
		if (context.performed)
		{
			Debug.Log("Select");
		}
	}
}