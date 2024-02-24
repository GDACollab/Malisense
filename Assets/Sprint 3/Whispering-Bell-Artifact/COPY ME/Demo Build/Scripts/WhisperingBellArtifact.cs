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
    private bool isActivated = false; // Prevents multiple activations before the end of the duration

    // Creates ripple effect on all enemies for a duration of 5 seconds
    public void WhisperBellAction()
    {
        isActivated = true;
        foreach (GameObject enemy in enemies)
        {
            Transform targetEnemy = enemy.transform;
            GameObject pulseObject = Instantiate(enemyPulseEffect, targetEnemy.position, targetEnemy.rotation); // Create target
            pulseObject.transform.parent = enemy.transform;
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
        Debug.Log(currentBellDuration);
        Debug.Log(currentBellCooldown);
        if (currentBellDuration > 0.0f) currentBellDuration += Time.deltaTime;
        if (currentBellDuration >= 5.0f)    
        {
            isActivated = false;
            currentBellCooldown += Time.deltaTime;
            currentBellDuration = 0.0f;
        }
        if (currentBellCooldown == 0.0f && currentBellDuration == 0.0f && !isActivated){ // Temporary key press, needs to be connected to player controller
            // TODO: add a call to make the player ripple effect
            Debug.Log("Bell Activated");
            WhisperBellAction();
        }
        if (currentBellCooldown > whisperingBellCooldown)currentBellCooldown = 0.0f;
        else if (currentBellCooldown > 0.0f) currentBellCooldown += Time.deltaTime;
    }
}
