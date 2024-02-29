using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(StateMachine_Updated))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SBProtoSightModule))]
public class SBProtoChase : StateBaseClass
{
    [Tooltip("Rate of acceleration.")]
    public float speed = 200f;

    [Tooltip("Amount of time that the sight beast will be able to track the player once visual contact is broken, measured in seconds.")]
    public float seeAroundWallsTime = 0.75f;

    private float _lastSeenTime;

    private StateMachine_Updated _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SBProtoSightModule _sight;
    private FearTracker _fear;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine_Updated>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _sight = GetComponent<SBProtoSightModule>();
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
        var visibility = _sight.GetTargetVisibility();
        // Target not visable
        if (visibility != SBProtoSightModule.Visibility.None)
        {
            _lastSeenTime = Time.time;
        }

        // Fear Functionality
        if (_fear)
        {
            _fear.AddFear(visibility != SBProtoSightModule.Visibility.None ? 0.8f : 0.3f);
        }

        // Begin chase
        if (visibility != SBProtoSightModule.Visibility.None
            || _lastSeenTime + seeAroundWallsTime > Time.time)
        {
            // Move towards target
            _pathfinder.SetTarget(_sight.target.position);

            // Look towards target
            //_sight.LookAt(_sight.target.position);
        }

        // Look either at the player, or in the direction of motion
        if (visibility != SBProtoSightModule.Visibility.None)
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
            _stateMachine.currentState = StateMachine_Updated.State.Patrolling;
        }
    }
}
