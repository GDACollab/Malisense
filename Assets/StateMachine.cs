using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Build an enemy base class as a state machine that returns either patrolling, Alert, and chasing.
*Put the 3 states into the inspector in a dropdown that sets the state of the enemy.
time meaning checking one state will uncheck another state.
*Allow other programmers to add scripts that handle the above states in the inspector.
*/

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Alert,
        Chasing
    }

    public State currentState;


    void Start()
    {
        currentState = State.Patrolling;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                // Patrol()
                break;
            case State.Alert:
                // Alert()
                break;
            case State.Chasing:
                // Chase()
                break;
        }
    }
}
