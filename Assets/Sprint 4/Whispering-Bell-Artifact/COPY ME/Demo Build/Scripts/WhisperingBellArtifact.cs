using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperingBellArtifact : MonoBehaviour
{
    private GameObject[] enemies;

    private bool blockAction = false;

    [Tooltip("Set the cooldown for the whispering bell artifact")]
    [SerializeField] public float whisperingBellCooldown;

    [Tooltip("Set how long the effects of the whispering bell artifact last on enemies")]
    [SerializeField] public float whisperingBellDuration;

    [SerializeField] public GameObject enemyPulseEffect;

    [SerializeField] public Artifact WhisperingBell; // Ideally would use this instead but it only updates once after every button press


    // Creates ripple effect on all enemies for a duration of 5 seconds
    public void WhisperBellAction()
    {
        Debug.Log("Do action");
        foreach (GameObject enemy in enemies)
        {
            Transform targetEnemy = enemy.transform;
            GameObject pulseObject = Instantiate(enemyPulseEffect, targetEnemy.position, targetEnemy.rotation); // Create target
            pulseObject.transform.parent = enemy.transform;
        }
        WhisperingBell.duration += Time.deltaTime;
        blockAction = true;
        
    }

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (WhisperingBell.cooldown > 0.0f || WhisperingBell.duration > 0.0f) Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        if (WhisperingBell.cooldown > whisperingBellCooldown)
        {
            WhisperingBell.cooldown = 0.0f;
            WhisperingBell.duration = 0.0f;
            Destroy(gameObject);
        }
        if (WhisperingBell.duration > 0.0f) WhisperingBell.duration += Time.deltaTime;
        if (WhisperingBell.duration >= 5.0f)    
        {
            WhisperingBell.cooldown += Time.deltaTime;
            WhisperingBell.duration = 0.0f;
        } 
        if (WhisperingBell.cooldown == 0.0f && WhisperingBell.duration == 0.0f && !blockAction){ // Temporary key press, needs to be connected to player controller
            // TODO: add a call to make the player ripple effect
            Debug.Log("Bell Activated");
            WhisperBellAction();
        }

        else if (WhisperingBell.cooldown > 0.0f) WhisperingBell.cooldown += Time.deltaTime;
    }
}
