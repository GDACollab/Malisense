using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Build an enemy base class as a state machine that returns either patrolling, Alert, and chasing.
*Put the 3 states into the inspector in a dropdown that sets the state of the enemy.
time meaning checking one state will uncheck another state.
*Allow other programmers to add scripts that handle the above states in the inspector.
*/

public class StateMachine_Improved : MonoBehaviour
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

    private bool alertInit = false;
    private bool patrolInit = false;
    private bool chaseInit = false;

    void Start()
    {
        currentState = State.Patrolling;
        initState(currentState);
    }

    void initState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Patrolling:
                patrol.Init();
                patrolInit = true;
                break;
            case State.Alert:
                alert.Init();
                alertInit = true;
                break;
            case State.Chasing:
                chasing.Init();
                chaseInit = true;
                break;
        }
    }

    public void switchState(State nextState)
    {
        currentState = nextState;
        switch (currentState)
        {
            case State.Patrolling:
                alertInit = false;
                chaseInit = false;
                if (!patrolInit)
                {
                    patrol.Init();
                    patrolInit = true;
                }
                if (patrol != null) { patrol.On_Update(); }
                break;
            case State.Alert:
                patrolInit = false;
                chaseInit = false;
                if (!alertInit)
                {
                    alert.Init();
                    alertInit = true;
                }
                if (alert != null) { alert.On_Update(); }
                break;
            case State.Chasing:
                patrolInit = false;
                alertInit = false;
                if (!chaseInit)
                {
                    chasing.Init();
                    chaseInit = true;
                }
                if (chasing != null) { chasing.On_Update(); }
                break;
        }
    }

    void Update()
    {
        switchState(currentState);
    }
}
