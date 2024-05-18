using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System.Drawing;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(SoundBeastSoundModule))]
public class SoundBeastAlert : StateBaseClass
{
    public float circleRadius = 3f;
    public float circleTime = 10f; // Time to circle around the player's position
    public float CirclingSpeed = 2f;
    private float angularSpeed = 0.7f;
    public TilemapCollider2D wallcollider;
    public float AlertedSpeed = 5f;

    public LayerMask wallLayers;

    [HideInInspector] public bool distractTarget;


    private float angle = 0;
    private StateMachine machine;
    public bool movedToOutside = false; // debug public
    private Transform player;
    private AIPath aiPath;
    public bool isCircling { get {return _sound.isCircling;} set{_sound.isCircling = value;}} // debug public
    public bool isMovingToOutside = false; // debug public
    private float circleStartTime;
    private Vector3 circleCenter;
    private Rigidbody2D rb;
    private Vector3 movePosition;
    private bool notcollided = false;

    private Player playerObj;
    private SoundBeastSoundModule _sound;
    private Vector3 soundPosition => _sound.detectedNoisePos;

    //get the noisePos variable from the soundBeast_noiseDetect_copy script and use it to pass the noise position to the Sound_Alert script

    private void Awake() {
        _sound = GetComponent<SoundBeastSoundModule>();
    }

    public override void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        machine = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        aiPath.enabled = true;
        isCircling = false;
        movedToOutside = false;
        if (distractTarget) distractTarget = false;
        else
        {
            circleCenter = soundPosition;
            aiPath.destination = soundPosition;
        }

        // Start pathfinding to player's position
        aiPath.maxSpeed = AlertedSpeed;
        aiPath.SearchPath();
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GetComponent<SoundBeastChase>().CancelInvoke();
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
                        movePosition = new Vector3(x, y, 0);
                        Vector3 direction = (movePosition - transform.position).normalized;    // Checking for obstacles between the enemy and the movePosition

                        // Check for obstacles between the enemy and the movePosition
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, movePosition), wallLayers);

                        // If the ray hits something tagged as "Wall"
                        if (hit.collider != null)                               // Checking for obstacles between the enemy and the movePosition
                        {                                                          
                            angle += angularSpeed * Time.deltaTime;
                        }
                        else if (hit.collider == null)                       // Checking for obstacles between the enemy and the movePosition
                        {
                            aiPath.destination = movePosition;
                            aiPath.SearchPath();
                            isMovingToOutside = true;
                        }

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
                    movePosition = new Vector3(x1, y1, 0);
                    //dont move if the movePosition is inside the wall
                    
                    while (wallcollider.bounds.Contains(movePosition))
                    {
                        if (notcollided == false)
                        {
                            notcollided = true;
                        }
                        angle += angularSpeed * Time.deltaTime;
                        x1 = circleCenter.x + Mathf.Cos(angle) * circleRadius;
                        y1 = circleCenter.y + Mathf.Sin(angle) * circleRadius;
                        movePosition = new Vector3(x1, y1, 0);
                    }
                    //move to new position that isnt in the wall
                    if (notcollided == true)
                    {
                        Vector3 direction = (movePosition - transform.position).normalized;             // Checking for obstacles between the enemy and the movePosition

                        // Check for obstacles between the enemy and the movePosition
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, movePosition), wallLayers);        // Checking for obstacles between the enemy and the movePosition

                        // If the ray hits something tagged as "Wall"
                        if (hit.collider != null)                       // Checking for obstacles between the enemy and the movePosition
                        {
                            angle += angularSpeed * Time.deltaTime;
                        }
                        else
                        {
                            aiPath.destination = movePosition;
                            aiPath.SearchPath();
                            notcollided = false;
                        }
                    }
                    //circle
                    if (!wallcollider.bounds.Contains(movePosition) && notcollided == false)
                    {
                        Vector3 direction = (movePosition - transform.position).normalized;         // Checking for obstacles between the enemy and the movePosition

                        // Check for obstacles between the enemy and the movePosition
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, movePosition), wallLayers);        // Checking for obstacles between the enemy and the movePosition

                        // If the ray hits something tagged as "Wall"
                        if (hit.collider != null)                               // Checking for obstacles between the enemy and the movePosition
                        {
                            angle += angularSpeed * Time.deltaTime;
                        }
                        else
                        {
                            aiPath.maxSpeed = CirclingSpeed;
                            aiPath.destination = movePosition;
                            aiPath.SearchPath();
                            if (aiPath.reachedDestination)
                            {
                                angle += angularSpeed * Time.deltaTime;

                            }
                        }

                    }
                }

            }
            
        }


        
    }

    //Sets target to player position when a monster is distracted
    public void SetDistractTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aiPath = GetComponent<AIPath>();
        distractTarget = true;
        circleCenter = soundPosition;
        aiPath.destination = soundPosition;
        aiPath.canMove = false;
    }

    //Sets target to it self when awoken from statue
    public void SetStatueTarget()
    {
        aiPath = GetComponent<AIPath>();
        distractTarget = true;
        circleCenter = transform.position;
        aiPath.destination = transform.position;
    }
}