using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateMachine_Updated))]
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

    [Tooltip("How long the enemy spends looking around once it reaches the player's last known location.")]
    public float lookAroundTime = 2f;

    [Tooltip("How many times a second the enemy turns its vision cone once it reaches the player's last known location.")]
    public float lookAroundSpeed = 4f;

    private float _gracePeriodRemaining;

    private StateMachine_Updated _stateMachine;
    private EnemyPathfinder _pathfinder;
    private SBProtoSightModule _sight;

    private Coroutine _lookAroundCoroutine;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine_Updated>();
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

            _sight.LookAt(_sight.target.position);
        }
        else
        {
            _pathfinder.acceleration = speed;

            // Make chase!
            if (_sight.GetTargetVisibility() != SBProtoSightModule.Visibility.None)
            {
                ExitToState(StateMachine_Updated.State.Chasing);
            }

            // Go back to patrol mode after a short animation
            else if (_pathfinder.AtGoal)
            {
                if (_lookAroundCoroutine == null)
                    _lookAroundCoroutine = StartCoroutine(LookAround());
            }
            else
            {
                _sight.LookInDirection(_pathfinder.direction);
            }
        }
    }

    private void ExitToState(StateMachine_Updated.State state)
    {
        if (_lookAroundCoroutine != null)
        {
            StopCoroutine(_lookAroundCoroutine);
            _lookAroundCoroutine = null;
        }
        _stateMachine.currentState = state;
    }

    private IEnumerator LookAround()
    {
        float timeLeft = lookAroundTime;
        while(timeLeft > 0f)
        {
            // Look in a random direction
            float rad = Random.value * Mathf.PI * 2f;
            _sight.LookInDirection(new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)));

            yield return new WaitForSeconds(Mathf.Min(1f / lookAroundSpeed, timeLeft));
            timeLeft -= 1f / lookAroundSpeed;
        }

        ExitToState(StateMachine_Updated.State.Patrolling);
    }
}
