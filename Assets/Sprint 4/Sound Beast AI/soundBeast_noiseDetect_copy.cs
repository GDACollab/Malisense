using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(PatrolPath))]
public class soundBeast_noiseDetect_copy : StateBaseClass
{
    public StateMachine_Improved machine;
    public PatrolPath patrolPath;
    private AIPath aiPath;

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

    private void Awake()
    {
        patrolPath = GetComponent<PatrolPath>();
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
        machine = GetComponent<StateMachine_Improved>();
        aiPath.destination = transform.position;
        aiPath.maxSpeed = maxSpeed;
        aiPath.SearchPath();
        _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

        // Start at the closest path point when entering patrol state
        var startArea = patrolPath.FindClosestArea(transform.position);
        _pathIndex = Array.IndexOf(patrolPath.areas, startArea);
    }

    public override void On_Update()
    {
        if(aiPath.reachedEndOfPath)
        {
            // Currently idle
            _idleTimeLeft -= Time.deltaTime;

            // When done idling, move to new area
            if(_idleTimeLeft < 0f)
            {
                _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

                if (patrolPath && patrolPath.areas.Length > 0)
                {
                    SBProtoPatrolArea nextArea = randomPath ? GetRandomArea() : GetNextArea();
                    aiPath.destination = nextArea.GetRandomPoint();
                    aiPath.SearchPath();
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject")
        {
            // Get the noise object and check that it has the noiseobject script
            scr_noiseObject noise = collision.GetComponent<scr_noiseObject>();
            if (noise == null)
            {
                Debug.LogError("ERROR: could not find scr_noiseObject in detected noise object \"" + collision.name + "\"");
                return;
            }

            // Check radius for noise level
            float loudness = noise.diameter;
            Vector2 noisePos = noise.transform.position;
            if (noise.parent.tag == "Player" && machine.currentState == StateMachine_Improved.State.Patrolling)
            {
                machine.switchState(StateMachine_Improved.State.Alert);
            }
        }
    }
}