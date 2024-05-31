using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SightBeastAnimations : MonoBehaviour
{
    public Animator _puppetAnimator;
    public Animator _lightAnimator;

    private StateMachine _stateMachine;
    private Rigidbody2D _rd2d;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
        //_animator = GetComponent<Animator>();
        _rd2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ----- Lighting animator inputs -----
        _lightAnimator.SetBool("Alert", _stateMachine.currentState == StateMachine.State.Alert);
        _lightAnimator.SetBool("Chasing", _stateMachine.currentState == StateMachine.State.Chasing);

        // ----- Puppet animator inputs -----
        // Visual response to startling events
        if (_stateMachine.currentState == StateMachine.State.Alert || _stateMachine.currentState == StateMachine.State.Distracted)
        {
            _puppetAnimator.SetTrigger("Alert");
        }

        // Feed speed data to animator
        _puppetAnimator.SetFloat("Speed", _rd2d.velocity.magnitude/3.5f);
    }
}
