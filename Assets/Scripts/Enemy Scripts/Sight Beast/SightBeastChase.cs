using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SightBeastSightModule))]
public class SightBeastChase : StateBaseClass
{
    [Tooltip("Rate of acceleration.")]
    public float speed = 200f;

    public float rateOfAcceleration = 1f;
    public float speedMax = 200f;

    [Tooltip("Amount of time that the sight beast will be able to track the player once visual contact is broken, measured in seconds.")]
    public float seeAroundWallsTime = 0.75f;

    private float _lastSeenTime;

    private StateMachine _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SightBeastSightModule _sight;
    private FearTracker _fear;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _sight = GetComponent<SightBeastSightModule>();

    }

    public override void Init()
    {
        _lastSeenTime = Time.time;
        _pathfinder.SetTarget(_sight.target.position);
        _pathfinder.acceleration = speed;

        _fear = _sight.target.GetComponent<FearTracker>();
    }

    public override void On_Update()
    {
        speed = speed * (1 + (rateOfAcceleration * Time.deltaTime));
        speed = Mathf.Min(speed, speedMax);
        _pathfinder.acceleration = speed;

        var seesTarget = _sight.CanSeeTarget();
        // Target not visable
        if (seesTarget)
        {
            _lastSeenTime = Time.time;
        }

        // Fear Functionality
        if (_fear)
        {
            _fear.AddFear(seesTarget ? 0.8f : 0.3f);
        }

        // Begin chase
        if (seesTarget || _lastSeenTime + seeAroundWallsTime > Time.time)
        {
            // Move towards target

            // Get point 10 units in front of player
            Vector3 dir = _sight.target.position - transform.position;

            _pathfinder.SetTarget(_sight.target.position + dir.normalized * 2);

            // Look towards target
            //_sight.LookAt(_sight.target.position);
        }

        // Look either at the player, or in the direction of motion
        if (seesTarget)
        {
            _sight.LookAt(_sight.target.position);
        }
        else
        {
            _sight.LookInDirection(_pathfinder.direction);
        }

        if(_pathfinder.AtGoal
            && Time.time > _lastSeenTime + seeAroundWallsTime)
        {
            _stateMachine.currentState = StateMachine.State.Patrolling;
        }
    }
}