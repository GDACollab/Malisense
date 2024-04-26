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
    private SpriteRenderer _monsterSprite;
    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _pathfinder = GetComponent<EnemyPathfinder>();
        
    }
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

    }

    public override void Init()
    {

        if (!_stateMachine.IsStatue()) StartCoroutine(Stunned());
        else
        {
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
        _pathfinder.SetTarget(_rb2d.position);
        _pathfinder.acceleration = 1000f;
        _rb2d.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(timeLeft);
        Debug.Log("Stunned ended");

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
        gameObject.tag = "Enemy";
        ExitToState(StateMachine.State.Alert);
        _rb2d.mass = 1;
        Debug.Log("Monster Working");
    }

}
