using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // for Image GameObject

public class PlayerStamina : MonoBehaviour
{
	// Stamina
	[SerializeField] float maxStamina;
	[Tooltip("n stamina per second")][SerializeField] float staminaRegen;
	[Tooltip("n stamina per second")][SerializeField] float staminaDepletion;
	[Tooltip("percentage of stamina required to sprint again")][Range(0.00f, 1.00f)][SerializeField] float minimumToSprint;
	float currentStamina;

	// UI Elements
	[Header("UI Elements")]
	public Image StaminaBar;

	// Stamina States
	[Header("Stamina States")]
	public bool isExhausted = false;		// makes it so player can't run; true when stamina is 0, false when currentStamina >= minimumToSprint

	// Player Movement Script (to know when player is sprinting)
	[Header("Player Movement Script")]
	[SerializeField] PlayerControl playerControl;

	void Start()
	{
		// Set Initial Stamina
		currentStamina = maxStamina;
	}

	void Update()
	{
		// Make sure currentStamina doesn't go over maxStamina
		if (currentStamina > maxStamina)
			currentStamina = maxStamina;

		// Check if player shouldn't be exhausted anymore
		if (currentStamina > maxStamina * minimumToSprint)
			isExhausted = false;

		// Check if player is sprinting
		if (playerControl.isSprinting)
		{
			currentStamina -= staminaDepletion * Time.deltaTime;		// deplete stamina
			if (currentStamina < 0f)                                    // check if player should now be exhausted
                isExhausted = true;
		}
		else
		{
			if (currentStamina < maxStamina)							// regen stamina
				currentStamina += staminaRegen * Time.deltaTime;
        }

        // Update the stamina bar
        StaminaBar.fillAmount = currentStamina / maxStamina;
    }
}
