using UnityEngine;
using Pathfinding;
using System.Linq;

[RequireComponent(typeof(StateMachine_Updated))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SBProtoSightModule))]
public class SBProtoPatrol : StateBaseClass
{
    public SBProtoPatrolArea[] searchPoints = new SBProtoPatrolArea[0];

    [Tooltip("Rate of acceleration.")]
    public float speed = 20f;

    [Tooltip("If the player is sighted twice within this duration, then skip straight to chase mode.")]
    public float alertDuration = 10f;

    [Tooltip("Minimum amount of time to stick in one spot before moving on, measured in seconds.")]
    public float minIdleTime = 3f;

    [Tooltip("Maximum amount of time to stick in one spot before moving on, measured in seconds.")]
    public float maxIdleTime = 6f;

    private float _idleTimeLeft;
    private float _lastSeenTime = float.NegativeInfinity;

    private StateMachine_Updated _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SBProtoSightModule _sight;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine_Updated>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _sight = GetComponent<SBProtoSightModule>();
    }

    private SBProtoPatrolArea GetRandomArea()
    {
        var areas = searchPoints.Where(area => !area.Contains(transform.position));
        if(areas.Count() == 0)
        {
            areas = searchPoints;
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

    public override void Init()
    {
        _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);
        _pathfinder.acceleration = speed;
        _pathfinder.SetTarget(transform.position);
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

                if (searchPoints.Length > 0)
                    _pathfinder.SetTarget(GetRandomArea().GetRandomPoint());
            }
        }

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
