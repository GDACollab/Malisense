using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class SoundAlert : StateBaseClass
{
    public float circleRadius = 3f;
    public float circleTime = 3f; // Time to circle around the player's position
    public StateMachine_Improved machine;

    private Transform player;
    private AIPath aiPath;
    public bool isCircling = false;
    private float circleStartTime;
    private Vector3 circleCenter;
    //get the noisePos variable from the soundBeast_noiseDetect_copy script and use it to pass the noise position to the Sound_Alert script
    

    public override void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aiPath = GetComponent<AIPath>();

        // Start pathfinding to player's position
        aiPath.destination = player.position;
    }



    public override void On_Update()
    {
        if (!isCircling && aiPath.reachedEndOfPath)
        {
            // Start circling around player's position
            isCircling = true;
            circleStartTime = Time.time;
            circleCenter = player.position;
        }

        if (isCircling)
        {
            // Check for circling time
            if (Time.time - circleStartTime > circleTime)
            {
                // Stop circling
                isCircling = false;
                machine.switchState(StateMachine_Improved.State.Patrolling);
            }
            /*
             * THIS CODE DOES NOT WORK AS INTENDED BUT ITS PLACEHOLDER FOR THE 
             * CIRCLE AROUND PLAYER FUNCTIONALITY
             * 
             * You can uncomment this code to test
            else
            {
                //rotate in place

                float angle = (Time.time - circleStartTime) * aiPath.maxSpeed; // Adjust speed of circling
                float x = Mathf.Cos(angle) * circleRadius + circleCenter.x;
                float z = Mathf.Sin(angle) * circleRadius + circleCenter.z;
                transform.position = new Vector3(x, transform.position.y, z);

            }
            */
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NoiseObject") && isCircling)
        {
            // Switch to chase state
            machine.switchState(StateMachine_Improved.State.Chasing);
        }
    }
}