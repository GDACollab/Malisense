using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(SoundBeastSoundModule))]
public class SoundBeastChase : StateBaseClass
{
    public Vector2 target;
    private Player player;
    
    public CircleCollider2D chaseCollider;
    public float detectionRadius = 5;

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
    private StateMachine machine;
    private bool hearsPlayer => _sound.heardSound;
    private float _chaseTimer;
    private SoundBeastSoundModule _sound;
    private Animator animator;

    private void Awake() {
        _sound = GetComponent<SoundBeastSoundModule>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        target = player.transform.position;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        machine = GetComponent<StateMachine>();
        GFX = GetComponentInChildren<SpriteRenderer>();
        GetComponentInChildren<Light2D>(true).gameObject.SetActive(true);
        
        chaseCollider.radius = detectionRadius;

        // Update pathfinding
        InvokeRepeating("UpdatePath",   // Method name
                        0f,             // Time to wait before method call
                        pathUpdateRate);// UpdatePath delay
        _fear = player.GetComponent<FearTracker>();
        _chaseTimer = chaseTimer;

        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", false);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target, OnPathComplete);
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
        if (path == null || path.vectorPath == null)
            return;

        // If exceeded current amount of waypoints in the path
        if (currentWaypoint+1 >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            target = player.transform.position;
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

        if (distance < nextWaypointDistance && currentWaypoint+1 < path.vectorPath.Count)
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
                chaseCollider.radius = 0;
                _sound.detectedNoisePos = target;
                machine.currentState = StateMachine.State.Alert;
            }
        }
        else{
            target = _sound.detectedNoisePos;
            _chaseTimer = chaseTimer;
        }
    }
}
