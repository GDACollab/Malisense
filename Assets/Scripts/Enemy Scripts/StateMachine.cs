using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

    private bool alertInit = false;
    private bool patrolInit = false;
    private bool chaseInit = false;
    private DungeonManager dungeonManager;
    void Start()
    {
        currentState = State.Patrolling;
        dungeonManager = FindObjectOfType<DungeonManager>();
    }

    void Update()
    {
        if(currentState!=State.Chasing && chaseInit){
            StopChase();
        }
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
                    SetChase();
                    chasing.Init();
                    chaseInit = true;
                }
                if (chasing != null) { chasing.On_Update(); }
                break;
        }
    }
    
    private void SetChase(){
        dungeonManager.AddEnemy(this);
    }
    
    private void StopChase(){
        dungeonManager.RemoveEnemy(this);
    }
}
