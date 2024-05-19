using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class SightBeastAnimations : MonoBehaviour
{
    public StateMachine _stateMachine;

    private Animator _animator;
    private Rigidbody2D _rd2d

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
        _animator = GetComponent<Animator>();
        _rd2d = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Visual response to startling events
        _animator.SetBool("Alert", _stateMachine.currentState == StateMachine.State.Alert);
        _animator.SetBool("Alert", _stateMachine.currentState == StateMachine.State.Distracted);

        // Feed speed data to animator
        _animator.SetFloat("Speed", _rd2d.velocity.magnitude);
    }
}
