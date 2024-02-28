using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SBProtoAnimations : MonoBehaviour
{
    public SBProtoStateMachine stateMachine;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Alert", stateMachine.currentState == SBProtoStateMachine.State.Alert);
        _animator.SetBool("Chasing", stateMachine.currentState == SBProtoStateMachine.State.Chasing);
    }
}
