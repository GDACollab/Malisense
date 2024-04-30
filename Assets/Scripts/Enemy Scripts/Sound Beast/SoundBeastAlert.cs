using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;


[RequireComponent(typeof(StateMachine))]
public class SoundBeastAlert : StateBaseClass
{
    public float circleRadius = 3f;
    public float circleTime = 10f; // Time to circle around the player's position
    public float angularSpeed = 0.7f;

    public float AlertSpeed = 5f;

    private float angle = 0;
    private StateMachine machine;
    public bool movedToOutside = false; // debug public
    private Transform player;
    private AIPath aiPath;
    public bool isCircling = false; // debug public
    public bool isMovingToOutside = false; // debug public
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
        movedToOutside = false;
        circleCenter = player.position;

        // Start pathfinding to player's position
        aiPath.maxSpeed = AlertSpeed;
        aiPath.destination = player.position;
        aiPath.SearchPath();
    }



    public override void On_Update()
    {
        if (!aiPath.pathPending && !isCircling && aiPath.reachedEndOfPath)
        {
            // Start circling around player's position
            circleStartTime = Time.time;
            angle = 0;
            isCircling = true;
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
                
                if (movedToOutside != true )
                {
                    if (!isMovingToOutside)
                    {
                        float x = circleCenter.x + Mathf.Cos(angle) * circleRadius;
                        float y = circleCenter.y + Mathf.Sin(angle) * circleRadius;
                        aiPath.destination = new Vector3(x, y, 0);
                        aiPath.SearchPath();
                        isMovingToOutside = true;
                    } else if  (aiPath.reachedEndOfPath && isMovingToOutside == true)
                    {
                        movedToOutside = true;
                        isMovingToOutside = false;
                    }
                    
                  
                }
                if (movedToOutside == true) 
                {
                    float x1 = circleCenter.x + Mathf.Cos(angle) * circleRadius;
                    float y1 = circleCenter.y + Mathf.Sin(angle) * circleRadius;
                    // This should be using the A* pathfinding, not direct position modification
                    transform.position = new Vector3(x1, y1, 0);
                    angle += angularSpeed * Time.deltaTime;
                }

            }
            
        }


        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (aiPath == null || collision == null) return;

        if (collision.tag == "NoiseObject" && !aiPath.pathPending && isCircling)
        {
            // Switch to chase state
            isCircling = false;
            machine.currentState = StateMachine.State.Chasing;
        }
    }
}