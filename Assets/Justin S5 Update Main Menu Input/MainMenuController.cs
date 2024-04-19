using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	[Header("Event System")]
	[SerializeField] EventSystem eventSystem;

	[Header("Default Selectables")]
	[SerializeField] Selectable mainMenuStartButton;
	[SerializeField] Selectable optionsVolumeSlider;
	[SerializeField] Selectable credits1NextButton;
	[SerializeField] Selectable credits2NextButton;
	[SerializeField] Selectable credits3PreviousButton;

	enum Screens { MAIN_MENU, OPTIONS, CREDITS1,  CREDITS2, CREDITS3 }
	Screens currentScreen = Screens.MAIN_MENU;


	public void Move(InputAction.CallbackContext context)
	{
		// when you press a button, there are 3 phases that happen: started, performed, and canceled
		// we don't want this function to run 3 times when the select button is pressed, so we check for the performed stage
		if (context.performed)
		{
			// there is no currently selected UI element
			if (eventSystem.currentSelectedGameObject == null)
			{
				// to prevent the default selectable from getting selected and then having the event system immediately select something else due to a navigation input
				// ex. on the main menu, when the S key is pressed, without the following line the start button would get selected and then the S key input would then make the options button selected instead
				eventSystem.sendNavigationEvents = false;

				SelectCurrentScreensDefaultSelectable();
			}

			// there is a currently selected UI element
			else
			{
				// revert the effects of the true clause of this if statement
				eventSystem.sendNavigationEvents = true;
			}
		}
	}

	public void Select(InputAction.CallbackContext context)
	{
		// when you press a button, there are 3 phases that happen: started, performed, and canceled
		// we don't want this function to run 3 times when the select button is pressed, so we check for the performed stage
		if (context.performed)
		{
			// there is no currently selected UI element
			if (eventSystem.currentSelectedGameObject == null)
			{
				SelectCurrentScreensDefaultSelectable();
			}

			// there is a currently selected UI element
			else
			{
				// attempt to get a button component from the currently selected UI element
				Button currentSelectedButton = eventSystem.currentSelectedGameObject.GetComponent<Button>();

				// check that we successfully retrieved a button component
				if (currentSelectedButton != null)
				{
					// invoke the button's on click behavior
					currentSelectedButton.onClick.Invoke();

					// if the button is one that brings up a new screen (ex. options, credits) then set new current screen and select the new current screen's default selectable
					SetNewCurrentScreen(eventSystem.currentSelectedGameObject);
					SelectCurrentScreensDefaultSelectable();
				}
			}
		}
	}

	void SetNewCurrentScreen(GameObject buttonGO)
	{
		string buttonGOName = buttonGO.name;
		string buttonGOScreenName = buttonGO.transform.parent.transform.parent.name;		// needed for the next and back buttons found in the credits screens

		switch (buttonGOName)
		{
			case "Options":
				currentScreen = Screens.OPTIONS;
				break;

			case "Credits":
				currentScreen = Screens.CREDITS1;
				break;

			case "ReturnButton":
				currentScreen = Screens.MAIN_MENU;
				break;

			case "NextCreditButton":
				if (buttonGOScreenName == "Credits")
				{
					currentScreen = Screens.CREDITS2;
				}
				else if (buttonGOScreenName == "Credits2")
				{
					currentScreen = Screens.CREDITS3;
				}
				break;

			case "BackCreditButton":
				if (buttonGOScreenName == "Credits2")
				{
					currentScreen = Screens.CREDITS1;
				}
				else if (buttonGOScreenName == "Credits3")
				{
					currentScreen = Screens.CREDITS2;
				}
				break;

			default:
				break;
		}
	}

	void SelectCurrentScreensDefaultSelectable()
	{
		switch (currentScreen)
		{
			case Screens.MAIN_MENU:
				mainMenuStartButton.Select();
				break;

			case Screens.OPTIONS:
				optionsVolumeSlider.Select();
				break;

			case Screens.CREDITS1:
				credits1NextButton.Select();
				break;

			case Screens.CREDITS2:
				credits2NextButton.Select();
				break;

			case Screens.CREDITS3:
				credits3PreviousButton.Select();
				break;
		}
	}
}