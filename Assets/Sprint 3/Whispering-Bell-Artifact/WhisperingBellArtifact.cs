using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperingBellArtifact : MonoBehaviour
{
    public Artifact whisperingBell;
    public GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    public float whisperingBellCooldown = 30.0f;
    public float whisperingBellDuration = 5.0f;

    // Checks if the artifact is on cooldown
    public bool IsCoolDown(Artifact artifact)
    {
        if (artifact.cooldown > 0.0f && artifact.cooldown < whisperingBellCooldown) return true;
        return false;
    }

    // Creates ripple effect on all enemies for a duration of 5 seconds
    public void WhisperBellAction(Artifact artifact)
    {
        while(artifact.duration < whisperingBellDuration)
        {
            foreach(GameObject enemy in enemies)
            {

            }
            artifact.duration+= Time.deltaTime;
        }
        artifact.duration = 0.0f;
        artifact.cooldown += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        // If button pressed and item is not on cooldown
        if (Input.GetKeyDown(KeyCode.C) && !IsCoolDown(whisperingBell)){ // Temporary key press, needs to be connected to player controller
            WhisperBellAction(whisperingBell);
        }
        if (!IsCoolDown(whisperingBell)) // Reset Cooldown when possible
        {
            whisperingBell.cooldown = 0.0f;
        }
        else
        {
            whisperingBell.cooldown+= Time.deltaTime;
        }
    }
}
