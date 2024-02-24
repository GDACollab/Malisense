using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperingBellArtifact : MonoBehaviour
{
    private GameObject[] enemies;

    [Tooltip("Set the cooldown for the whispering bell artifact")]
    [SerializeField] public float whisperingBellCooldown;

    [Tooltip("Set how long the effects of the whispering bell artifact last on enemies")]
    [SerializeField] public float whisperingBellDuration;

    [SerializeField] public GameObject enemyPulseEffect;

    private float currentBellCooldown = 0f;
    private float currentBellDuration = 0f;  

    // Checks if the artifact is on cooldown
    public bool IsCoolDown()
    {
        if (currentBellCooldown > 0.0f && currentBellCooldown < whisperingBellCooldown) return true;
        return false;
    }

    // Creates ripple effect on all enemies for a duration of 5 seconds
    public void WhisperBellAction()
    {
        currentBellDuration += 1;
        foreach (GameObject enemy in enemies)
        {
            Transform targetEnemy = enemy.transform;
            Instantiate(enemyPulseEffect, targetEnemy.position, targetEnemy.rotation); // Create target
        }
        currentBellDuration += Time.deltaTime;
    }

    private void Start()
    {
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("Updating my script");
        if (currentBellDuration > 0.0f)
        {
            currentBellDuration += Time.deltaTime;
        }
        else if (currentBellCooldown > 5.0f)    
        {
            currentBellCooldown += Time.deltaTime;
            currentBellDuration = 0.0f;
        }
        // If button pressed and item is not on cooldown
        if (currentBellCooldown == 0.0f){ // Temporary key press, needs to be connected to player controller
            // TODO: add a call to make the pulse effect
            Debug.Log("Bell Activated");
            WhisperBellAction();
        }
        if (currentBellCooldown > whisperingBellCooldown) // Reset Cooldown when possible
        {
            Debug.Log("Reset Bell Cooldown");
            currentBellCooldown = 0.0f;
        }
        else if (currentBellCooldown > 0.0f)
        {
        currentBellCooldown += Time.deltaTime;
        }
    }
}
