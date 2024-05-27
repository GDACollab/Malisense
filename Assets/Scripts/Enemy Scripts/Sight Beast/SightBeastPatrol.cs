using UnityEngine;
using System.Linq;
using Array = System.Array;
using FMODUnity;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SightBeastSightModule))]
public class SightBeastPatrol : StateBaseClass
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

    private StateMachine _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SightBeastSightModule _sight;
    private int _pathIndex;

    //emitter that audio is played from
    private StudioEventEmitter _audioEmitter;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _sight = GetComponent<SightBeastSightModule>();
        _audioEmitter = GetComponent<StudioEventEmitter>();
    }

    private SBProtoPatrolArea GetRandomArea()
    {
        var areas = patrolPath.areas.Where(area => !area.Contains(transform.position));
        if (areas.Count() == 0)
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
        _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);
        _pathfinder.acceleration = speed;
        _pathfinder.SetTarget(transform.position);

        // Start at the closest path point when entering patrol state
        var startArea = patrolPath.FindClosestArea(transform.position);
        _pathIndex = Array.IndexOf(patrolPath.areas, startArea);
    }

    public override void On_Update()
    {
        if (_pathfinder.AtGoal)
        {
            // Currently idle
            _idleTimeLeft -= Time.deltaTime;

            // When done idling, move to new area
            if (_idleTimeLeft < 0f)
            {
                //Sound effect for patrolling
                GameObject.Find("Global Teapot").GetComponent<AudioManager>().PlaySightIdleSFX(_audioEmitter);
                _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

                if (patrolPath && patrolPath.areas.Length > 0)
                {
                    SBProtoPatrolArea nextArea = randomPath ? GetRandomArea() : GetNextArea();
                    _pathfinder.SetTarget(nextArea.GetRandomPoint());
                }
            }
        }

        _sight.LookInDirection(_pathfinder.direction);

        // Switch to alert state
        if (_sight.CanSeeTarget())
        {
            //Sound effect for starting chase
            GameObject.Find("Global Teapot").GetComponent<AudioManager>().PlaySightAlertSFX(_audioEmitter);
            if (_lastSeenTime + alertDuration > Time.time)
            {
                _stateMachine.currentState = StateMachine.State.Chasing;
            }
            else
            {
                _stateMachine.currentState = StateMachine.State.Alert;
            }

            _lastSeenTime = Time.time;
        }


    }

    public void checkIfVisible(GameObject dynamite)
    {
        scr_noiseObject noise = dynamite.GetComponent<scr_noiseObject>();
        if (noise.noiseDistractsSound == true)
        {
            if (_sight.CanSee(dynamite.transform.position, _sight.targetRadius))
            {
                _stateMachine.currentState = StateMachine.State.Alert;
            }
        }
    }
}