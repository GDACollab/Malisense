using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(PatrolPath))]
[RequireComponent(typeof(StateMachine))]
public class SoundBeastPatrol : StateBaseClass
{
    private StateMachine machine;
    public PatrolPath patrolPath;
    private AIPath aiPath;
    private Animator animator;

    [Tooltip("Rate of acceleration.")]
    public float maxSpeed = 2f;

    [Tooltip("If the player is sighted twice within this duration, then skip straight to chase mode.")]
    public float alertDuration = 10f;

    [Tooltip("Minimum amount of time to stick in one spot before moving on, measured in seconds.")]
    public float minIdleTime = 3f;

    [Tooltip("Maximum amount of time to stick in one spot before moving on, measured in seconds.")]
    public float maxIdleTime = 6f;

    [Tooltip("Move to random areas in the path rather than following a set order.")]
    public bool randomPath;

    private float _idleTimeLeft;
    private float _lastSeenTime = float.NegativeInfinity;

    private int _pathIndex;

    private Player playerObj;



    private void Awake()
    {
        patrolPath = GetComponent<PatrolPath>();
        animator = GetComponentInChildren<Animator>();
    }

    private SBProtoPatrolArea GetRandomArea()
    {
        var areas = patrolPath.areas.Where(area => !area.Contains(transform.position));
        if(areas.Count() == 0)
        {
            areas = patrolPath.areas;
        }

        float maxWeight = areas.Sum(area => area.weight);
        float value = maxWeight * Random.value;

        foreach (var area in areas)
        {
            value -= area.weight;
            if (value <= 0)
                return area;
        }

        return areas.First();
    }

    private SBProtoPatrolArea GetNextArea()
    {
        return patrolPath.areas[_pathIndex++ % patrolPath.areas.Length];
    }

    public override void Init()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.enabled = true;
        machine = GetComponent<StateMachine>();
        aiPath.destination = transform.position;
        aiPath.maxSpeed = maxSpeed;
        aiPath.SearchPath();
        _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

        // Start at the closest path point when entering patrol state
        var startArea = patrolPath.FindClosestArea(transform.position);
        _pathIndex = Array.IndexOf(patrolPath.areas, startArea);
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void On_Update()
    {
        
        if(aiPath.reachedEndOfPath)
        {
            // Currently idle
            _idleTimeLeft -= Time.deltaTime;
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);

            // When done idling, move to new area
            if (_idleTimeLeft < 0f)
            {
                _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);

                if (patrolPath && patrolPath.areas.Length > 0)
                {
                    SBProtoPatrolArea nextArea = randomPath ? GetRandomArea() : GetNextArea();
                    aiPath.destination = nextArea.GetRandomPoint();
                    aiPath.SearchPath();
                }
            }
        }
    }
}