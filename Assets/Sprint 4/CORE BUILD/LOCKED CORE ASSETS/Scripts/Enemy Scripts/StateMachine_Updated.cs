using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Build an enemy base class as a state machine that returns either patrolling, Alert, and chasing.
*Put the 3 states into the inspector in a dropdown that sets the state of the enemy.
time meaning checking one state will uncheck another state.
*Allow other programmers to add scripts that handle the above states in the inspector.
*/

public class StateMachine_Updated : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Alert,
        Chasing
    }

    // Tracks which state the monster is in or is about to switch to
    public State currentState;

    // Tracks the last state that Init was called on
    private State lastState;

    public StateBaseClass patrol;
    public StateBaseClass chasing;
    public StateBaseClass alert;
    private AudioManager audioManager;
    void Start()
    {
        switchState(currentState);
        audioManager = GameObject.FindGameObjectWithTag("GlobalTeapot").GetComponent<AudioManager>();
    }

    public void switchState(State newState)
    {
        currentState = newState;
        lastState = newState;

        switch (newState)
        {
            case State.Patrolling:
                patrol.Init();
                break;
            case State.Alert:
                alert.Init();
                break;
            case State.Chasing:
                audioManager.Play(audioManager.monsterScream);
                chasing.Init();
                break;
        }
    }

    void Update()
    {
        if (lastState != currentState)
            switchState(currentState);

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
