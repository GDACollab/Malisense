using UnityEngine;
using Pathfinding;
using System.Linq;

[RequireComponent(typeof(StateMachine_Updated))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SBProtoSightModule))]
public class SBProtoPatrol : StateBaseClass
{
    public PatrolPath patrolPath;

    [Tooltip("Rate of acceleration.")]
    public float speed = 20f;

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

    private StateMachine_Updated _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SBProtoSightModule _sight;
    private int _pathIndex;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine_Updated>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _sight = GetComponent<SBProtoSightModule>();
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
        return patrolPath.areas[_pathIndex++ % patrolPath.areas.Count];
    }

    public override void Init()
    {
        _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);
        _pathfinder.acceleration = speed;
        _pathfinder.SetTarget(transform.position);

        // Start at the closest path point when entering patrol state
        var startArea = patrolPath.FindClosestArea(transform.position);
        _pathIndex = patrolPath.areas.IndexOf(startArea);
    }

    public override void On_Update()
    {
        if(_pathfinder.AtGoal)
        {
            // Currently idle
            _idleTimeLeft -= Time.deltaTime;

            // When done idling, move to new area
            if(_idleTimeLeft < 0f)
            {
                _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

                if (patrolPath && patrolPath.areas.Count > 0)
                {
                    SBProtoPatrolArea nextArea = randomPath ? GetRandomArea() : GetNextArea();
                    _pathfinder.SetTarget(nextArea.GetRandomPoint());
                }
            }
        }

        _sight.LookInDirection(_pathfinder.direction);

        // Switch to alert state
        if (_sight.GetTargetVisibility() != SBProtoSightModule.Visibility.None)
        {
            if(_lastSeenTime + alertDuration > Time.time)
            {
                _stateMachine.currentState = StateMachine_Updated.State.Chasing;
            }
            else
            {
                _stateMachine.currentState = StateMachine_Updated.State.Alert;
            }

            _lastSeenTime = Time.time;
        }
    }
}
