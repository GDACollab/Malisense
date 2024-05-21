using UnityEngine;

public class scr_playerNoiseTrack : MonoBehaviour
{
    //This script controls when a sound is made by the player when moving
    public GameObject player;
    //public PlayerControl playerMovement;
    public scr_noise noiseSystem;
    public float NoiseFrequency = 0.2f;
    float timeCheck = 0;
    public float walkLoudness;
    public float sprintLoudness;
    //public float sneakLoudness;

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerControl>().isMoving) //When the player is moving
        {
            timeCheck += Time.deltaTime; //increment time
            while (timeCheck >= NoiseFrequency) //When enough time has passed
            {                                   //(and repetition if there has been too much lag)
                float size = walkLoudness; // Decide size of noiceObject based on current state
                if (player.GetComponent<PlayerControl>().isSprinting) { size = sprintLoudness; }
                //else if(player.GetComponent<PlayerControl>().isSneaking) { size = sneakLoudness; }
                noiseSystem.MakeSound(player.transform.position,size); //Send command to create sound object
                timeCheck -= NoiseFrequency; //decrement timeCheck to prevent infinite loop!
            }
        }
        else //When not moving
        {
            //Make sure time is increased such that when the player moves again, they instantly make sound
            timeCheck = Mathf.Min(NoiseFrequency, timeCheck += Time.deltaTime);
        }
    }
}
