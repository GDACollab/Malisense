using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperingBellArtifact : MonoBehaviour
{
    public Artifact whisperingBell;
    public GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    // Checks if the artifact is on cooldown
    public bool isCoolDown(Artifact artifact)
    {
        // Need set it to actual time
        if (artifact.cooldown > 0.0 && artifact.cooldown < 31.0) return true;
        return false;
    }

    public void whisperBellAction(Artifact artifact)
    {
        // Need to set to time in sec
        while(artifact.duration < 5.0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                return;
            }
            artifact.duration++;
        }
        artifact.duration = 0;
        artifact.cooldown++;
    }
    // Update is called once per frame
    void Update()
    {  
        if (!isCoolDown(whisperingBell))
        {
            whisperingBell.cooldown = 0;
        }
        else
        {
            whisperingBell.cooldown++;
        }
    }
}
