using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]

public class ScentBeastDistract : StateBaseClass, ISwitchable
{
    // Start is called before the first frame update
    [Tooltip("Amount of time monster is distracted")] public float distractLength;

    private Coroutine _stunnedCoroutine;
    private StateMachine _stateMachine;
    private EnemyPathfinder _pathfinder;
    private int _statueCounter;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _pathfinder = GetComponent<EnemyPathfinder>();

    }
    void Start()
    {

    }

    public override void Init()
    {
        //If not statue stop monster
        if (!_stateMachine.IsStatue())
        {
            _pathfinder.SetTarget(transform.position);
            _pathfinder.acceleration = 0;
            //Set target to player position at the time of being distracted
            //Stun monster
            StartCoroutine(Stunned());
        }
        //If statue set statue counter and set sprite to statue sprite
        else
        {
            _statueCounter = _stateMachine.GetActivationsToAwake();
            _stateMachine.CreateStatue();
        }
    }
    public override void On_Update()
    {

    }

    //Stuns monster for distract length and sets back to alert after
    private IEnumerator Stunned()
    {
        float timeLeft = distractLength;
        //Debug.Log("Stunned started");
        yield return new WaitForSeconds(timeLeft);
        //Debug.Log("Stunned ended");
        ExitToState(StateMachine.State.Patrolling);
    }

    private void ExitToState(StateMachine.State state)
    {
        if (_stunnedCoroutine != null)
        {
            StopCoroutine(_stunnedCoroutine);
            _stunnedCoroutine = null;
        }
        _stateMachine.currentState = state;
    }
    public void SwitchInit(bool activated)
    {

    }

    public void SwitchInteract(bool activated)
    {
        //If switch is activated subtract from statue counter
        if (activated) _statueCounter--;
        else _statueCounter++;
        //Once statue counter reaches 0 reset monster to normal and set to alert
        if (_statueCounter <= 0)
        {
            _stateMachine.AwakenStatue();
            //Alert target is set to it self's position
            ExitToState(StateMachine.State.Patrolling);
            //Debug.Log("Monster Working");
        }
    }

}
