using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading;

public class SoundChase : StateBaseClass
{
    public Transform target;

    public float chaseTimer = 5f;
    public float speed = 200f;              // Acceleration speed
    public float nextWaypointDistance = 3f; // How close an enemy needs to be to the waypoint to move on to next one
    public float pathUpdateRate = 0.5f;     // Time between pathfinder updates

    public SpriteRenderer GFX;

    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    private FearTracker _fear;
    private StateMachine_Improved machine;
    private bool hearsPlayer = false;
    private float _chaseTimer;
    

    public override void Init()
    {
        target = GameObject.FindWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        machine = GetComponent<StateMachine_Improved>();
        GFX = GetComponentInChildren<SpriteRenderer>();

        // Update pathfinding
        InvokeRepeating("UpdatePath",   // Method name
                        0f,             // Time to wait before method call
                        pathUpdateRate);// UpdatePath delay
        _fear = target.GetComponent<FearTracker>();
        _chaseTimer = chaseTimer;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;    // Start at beginning of new path
        }
    }

    public override void On_Update()
    {
        if (path == null)
            return;

        // If exceeded current amount of waypoints in the path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Find direction to waypoint
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;  // Move on to next waypoint
        }
        
        // Rotation
        // float angle = Vector2.SignedAngle(Vector2.right, direction) - 90;
        // if (direction.magnitude > 0)
        //     rb.MoveRotation(angle);

        // ----- Flip Sprite -----
        // Flip left
        if (force.x >= 0.01f)
        {
            GFX.flipX = true;
        }
        // Flip right
        else if (force.x <= -0.01f)
        {
            GFX.flipX = false;
        }
        
        // Fear Functionality
        if (_fear)
        {
            _fear.AddFear(hearsPlayer ? 0.8f : 0.3f);
        }
        
        if(hearsPlayer==false){
            _chaseTimer -= Time.deltaTime;
            if(_chaseTimer <=0){
                _chaseTimer = chaseTimer;
                CancelInvoke();
                machine.switchState(StateMachine_Improved.State.Alert);
            }
        }
        else{
            _chaseTimer = chaseTimer;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject")
        {
            hearsPlayer = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject")
        {
            hearsPlayer = false;
        }
    }
}
