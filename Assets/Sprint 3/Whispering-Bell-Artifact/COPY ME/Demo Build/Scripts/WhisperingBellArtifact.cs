using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperingBellArtifact : MonoBehaviour
{
    public Artifact whisperingBell;
    public GameObject[] enemies;

    [Tooltip("Set the cooldown for the whispering bell artifact")]
    [SerializeField] public float whisperingBellCooldown;

    [Tooltip("Set how long the effects of the whispering bell artifact last on enemies")]
    [SerializeField] public float whisperingBellDuration;

    [Tooltip("Set the location and name of the Target Prefab")]
    [SerializeField] public GameObject enemyVisualizer;

    // Checks if the artifact is on cooldown
    public bool IsCoolDown(Artifact artifact)
    {
        if (artifact.cooldown > 0.0f && artifact.cooldown < whisperingBellCooldown) return true;
        return false;
    }

    // Creates ripple effect on all enemies for a duration of 5 seconds
    public void WhisperBellAction(Artifact artifact)
    {
        foreach (GameObject enemy in enemies)
        {
            Transform targetEnemy = enemy.transform;
            Instantiate(enemyVisualizer, targetEnemy.position, targetEnemy.rotation); // Create target
        }
        while (artifact.duration < whisperingBellDuration)
        {
            artifact.duration += Time.deltaTime;
        }
        artifact.duration = 0.0f;
        artifact.cooldown += Time.deltaTime;
    }

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        // If button pressed and item is not on cooldown
        if (Input.GetKeyDown(KeyCode.C) && !IsCoolDown(whisperingBell)){ // Temporary key press, needs to be connected to player controller
            // TODO: add a call to make the pulse effect
            WhisperBellAction(whisperingBell);
        }
        if (!IsCoolDown(whisperingBell)) // Reset Cooldown when possible
        {
            whisperingBell.cooldown = 0.0f;
        }
        else
        {
            whisperingBell.cooldown += Time.deltaTime;
        }
    }
}
