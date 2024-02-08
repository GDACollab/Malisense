using UnityEngine;

[RequireComponent(typeof(SBProtoStateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]
[RequireComponent(typeof(SBProtoSightModule))]
public class SBProtoAlert : StateBaseClass
{
    [Tooltip("Seconds after entering alert during which the player cannot be chased.")]
    public float gracePeriod = 0.5f;

    [Tooltip("Rate of acceleration.")]
    public float speed = 200f;

    [Tooltip("How close an enemy needs to be to the waypoint to move on to next one.")]
    public float nextWaypointDistance = 3f;

    private float _gracePeriodRemaining;

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
        _gracePeriodRemaining = gracePeriod;
        
        _pathfinder.SetTarget(_sight.target.position);
        _pathfinder.acceleration = 0f;
    }

    public override void On_Update()
    {
        if (_gracePeriodRemaining > 0f)
        {
            // Delay before investigating the sight
            _gracePeriodRemaining = Mathf.Max(_gracePeriodRemaining - Time.deltaTime, 0f);

            if (_sight.GetTargetVisibility() != SBProtoSightModule.Visibility.None)
            {
                _pathfinder.SetTarget(_sight.target.position);
            }
        }
        else
        {
            _pathfinder.acceleration = speed;

            // Make chase!
            if (_sight.GetTargetVisibility() != SBProtoSightModule.Visibility.None)
            {
                _stateMachine.SwitchState(SBProtoStateMachine.State.Chasing);
            }

            // Go back to patrol mode
            else if (_pathfinder.AtGoal)
            {
                _stateMachine.SwitchState(SBProtoStateMachine.State.Patrolling);
            }
        }
    }
}
