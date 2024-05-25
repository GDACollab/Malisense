using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Scent_Beast_Animations : MonoBehaviour
{
    public Animator animator;

    private StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Alert", stateMachine.currentState == StateMachine.State.Alert);
        animator.SetBool("Chasing", stateMachine.currentState != StateMachine.State.Patrolling && stateMachine.currentState != StateMachine.State.Alert);
        animator.SetBool("Patrol", stateMachine.currentState == StateMachine.State.Patrolling);
    }
}
