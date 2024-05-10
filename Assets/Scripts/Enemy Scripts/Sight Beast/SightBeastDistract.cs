using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]

public class SightBeastDistract : StateBaseClass, ISwitchable
{
    // Start is called before the first frame update
    [Tooltip("Amount of time monster is distracted")] public float distractLength;

    private Coroutine _stunnedCoroutine;
    private StateMachine _stateMachine;
    private Rigidbody2D _rb2d;
    private EnemyPathfinder _pathfinder;
    private SightBeastAlert _alert;
    private SpriteRenderer _sightSprite;
    private Sprite AwakeSprite;
    private int _statueCounter;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _alert = GetComponent<SightBeastAlert>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        
    }
    void Start()
    {
        _sightSprite = GetComponentInChildren<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        AwakeSprite = _sightSprite.sprite;

    }

    public override void Init()
    {
        //If not statue stop monster
        if (!_stateMachine.IsStatue())
        {
            _pathfinder.SetTarget(transform.position);
            _pathfinder.acceleration = 0;
            //Set target to player position at the time of being distracted
            _alert.SetDistractTarget();
            //Stun monster
            StartCoroutine(Stunned());
        }
        //If statue set statue counter and set sprite to statue sprite
        else
        {
            _statueCounter = _stateMachine.GetActivationsToAwake();
            _sightSprite.sprite = _stateMachine.GetStatueSprite();
            //Stops enemy from hurting player while statue
            gameObject.tag = "Untagged";
            //Prevents player from pushing monster while statue
            _rb2d.mass = 10000;
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
        _pathfinder.acceleration = _alert.speed;
        ExitToState(StateMachine.State.Alert);
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
            _sightSprite.sprite = AwakeSprite;
            gameObject.tag = "Enemy";
            //Alert target is set to it self's position
            _alert.SetStatueTarget();
            ExitToState(StateMachine.State.Alert);
            _rb2d.mass = 1;
            //Debug.Log("Monster Working");
        }
    }

}