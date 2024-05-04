using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(EnemyPathfinder))]

public class SightBeastDistract : StateBaseClass, ISwitchable
{
    // Start is called before the first frame update

    public float distractLength;
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

        if (!_stateMachine.IsStatue())
        {
            _pathfinder.SetTarget(transform.position);
            _pathfinder.acceleration = 0;
            _alert.SetDistractTarget();
            //_pathfinder.acceleration  = _alert.speed;
            StartCoroutine(Stunned());
        }
        else
        {
            _statueCounter = _stateMachine.StatueCounter;
            _sightSprite.sprite = _stateMachine.SpriteStatue;
            gameObject.tag = "Untagged";
            _rb2d.mass = 10000;
        }
    }
    public override void On_Update()
    {

    }
    private IEnumerator Stunned()
    {
        float timeLeft = distractLength;
        Debug.Log("Stunned started");
        yield return new WaitForSeconds(timeLeft);
        Debug.Log("Stunned ended");
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
        _statueCounter--;
        if (_statueCounter <= 0)
        {
            _stateMachine.AwakenStatue();
            _sightSprite.sprite = AwakeSprite;
            gameObject.tag = "Enemy";
            _alert.SetStatueTarget();
            ExitToState(StateMachine.State.Alert);
            _rb2d.mass = 1;
            Debug.Log("Monster Working");
        }
    }

}
