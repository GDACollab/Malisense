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

    public StateBaseClass patrol;
    public StateBaseClass chasing;
    public StateBaseClass alert;

    void Start()
    {
        // 
        // currentState = State.Patrolling;
        // switchState(State.Alert);
    }

    void switchState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Patrolling:
                patrol.Init();
                break;
            case State.Alert:
                alert.Init();
                break;
            case State.Chasing:
                chasing.Init();
                break;
        }
    }

    void Update()
    { 
        switch (currentState)
        {
            case State.Patrolling:
                if (patrol != null) { patrol.On_Update(); }
                break;
            case State.Alert:
                if(alert != null) { alert.On_Update(); }
                break;
            case State.Chasing:
                if (chasing != null) { chasing.On_Update(); }
                break;
        }
    }
}
