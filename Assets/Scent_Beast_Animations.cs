using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Scent_Beast_Animations : MonoBehaviour
{
    public StateMachine stateMachine;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("see player", stateMachine.currentState == StateMachine.State.Alert);
        _animator.SetFloat("running speed", );
    }
}
