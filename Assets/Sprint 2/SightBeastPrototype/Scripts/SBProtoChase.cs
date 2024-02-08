using UnityEngine;

[RequireComponent(typeof(SBProtoStateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SBProtoSightModule))]
public class SBProtoChase : StateBaseClass
{
    [Tooltip("Rate of acceleration.")]
    public float speed = 200f;

    [Tooltip("Amount of time that the sight beast will be able to track the player once visual contact is broken, measured in seconds.")]
    public float seeAroundWallsTime = 0.75f;

    private float _lastSeenTime;

    private SBProtoStateMachine _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SBProtoSightModule _sight;

    private void Awake()
    {
        _stateMachine = GetComponent<SBProtoStateMachine>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        _sight = GetComponent<SBProtoSightModule>();
    }

    public override void Init()
    {
        _lastSeenTime = Time.time;
        _pathfinder.SetTarget(_sight.target.position);
        _pathfinder.acceleration = speed;
    }

    public override void On_Update()
    {
        var visibility = _sight.GetTargetVisibility();
        if (visibility != SBProtoSightModule.Visibility.None)
        {
            _lastSeenTime = Time.time;
        }

        if (visibility != SBProtoSightModule.Visibility.None
            || _lastSeenTime + seeAroundWallsTime > Time.time)
        {
            _pathfinder.SetTarget(_sight.target.position);
        }

        if(_pathfinder.AtGoal
            && Time.time > _lastSeenTime + seeAroundWallsTime)
        {
            _stateMachine.SwitchState(SBProtoStateMachine.State.Patrolling);
        }
    }
}
