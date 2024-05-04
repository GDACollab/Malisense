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
        Chasing,
        Distracted
    }

    public State currentState;
    public Sprite SpriteStatue;

    public StateBaseClass patrol;
    public StateBaseClass chasing;
    public StateBaseClass alert;
    public StateBaseClass distracted;

    public bool Statue;
    public int StatueCounter;
    private bool alertInit = false;
    private bool patrolInit = false;
    private bool chaseInit = false;
    private bool distractInit = false;
    private bool _statue;
    private AudioManager audioManager;
    void Start()
    {
        _statue = Statue;
        if(_statue) currentState = State.Distracted;
        else currentState = State.Patrolling;
        audioManager = GameObject.FindGameObjectWithTag("Global Teapot").GetComponent<AudioManager>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                alertInit = false;
                chaseInit = false;
                distractInit = false;
                if (!patrolInit)
                {
                    Debug.Log("Patrol Start");
                    patrol.Init();
                    patrolInit = true;
                }
                if (patrol != null) { patrol.On_Update(); }
                break;
            case State.Alert:
                patrolInit = false;
                chaseInit = false;
                distractInit = false;
                if (!alertInit)
                {
                    Debug.Log("Alert Start");
                    alert.Init();
                    alertInit = true;
                }
                if (alert != null) { alert.On_Update(); }
                break;
            case State.Chasing:
                patrolInit = false;
                alertInit = false;
                distractInit = false;
                if (!chaseInit)
                {
                    Debug.Log("Chase Start");
                    audioManager.Play(audioManager.monsterScream);
                    chasing.Init();
                    chaseInit = true;
                }
                if (chasing != null) { chasing.On_Update(); }
                break;
            case State.Distracted:
                patrolInit = false;
                alertInit = false;
                chaseInit = false;
                if (!distractInit)
                {
                    Debug.Log("Distract Start");
                    distracted.Init();
                    distractInit = true;
                }
                if (distracted != null) { distracted.On_Update(); }
                break;
        }
    }

    public bool IsStatue() => _statue;

    public void AwakenStatue() => _statue = false;
}

