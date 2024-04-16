using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;


[RequireComponent(typeof(StateMachine))]
public class SoundBeastAlert : StateBaseClass
{
    public float circleRadius = 3f;
    public float circleTime = 10f; // Time to circle around the player's position
    public float angularSpeed = 0.7f;
    private float angle = 0;
    private StateMachine machine;

    private Transform player;
    private AIPath aiPath;
    public bool isCircling = false;
    private float circleStartTime;
    private Vector3 circleCenter;
    private Rigidbody2D rb;

    //get the noisePos variable from the soundBeast_noiseDetect_copy script and use it to pass the noise position to the Sound_Alert script
    

    public override void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        machine = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        isCircling = false;

        // Start pathfinding to player's position
    
        aiPath.destination = player.position;
        aiPath.SearchPath();
    }



    public override void On_Update()
    {
        if (!aiPath.pathPending && !isCircling && aiPath.reachedEndOfPath)
        {
            // Start circling around player's position
            isCircling = true;
            circleCenter = rb.position;
            circleStartTime = Time.time;
            angle = 0;
        }

        if (!aiPath.pathPending && isCircling)
        {
            // Check for circling time
            if (Time.time - circleStartTime > circleTime)
            {
                // Stop circling
                isCircling = false;
                machine.currentState = StateMachine.State.Patrolling;
            }
            
            
            else
            {
                //rotate in place

                angle += Time.deltaTime * angularSpeed; // Adjust speed of circling
                angle %= 2 * Mathf.PI;
                float x = Mathf.Cos(angle) * circleRadius + circleCenter.x;
                float y = Mathf.Sin(angle) * circleRadius + circleCenter.y;
                transform.position = new Vector2(x, y);

            }
            
        }


        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject" && !aiPath.pathPending && isCircling)
        {
            // Switch to chase state
            isCircling = false;
            machine.currentState = StateMachine.State.Chasing;
        }
    }
}