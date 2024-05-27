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
        Chasing,
        Distracted
    }

    public State currentState;
    [Tooltip("Sets which sprite the monster starts in when Statue is true.")]
    public Sprite SpriteStatue;

    public StateBaseClass patrol;
    public StateBaseClass chasing;
    public StateBaseClass alert;
    public StateBaseClass distracted;

    [Tooltip("If true monster starts as a statue at start.")]
    public bool Statue;
    [Tooltip("Amount of activated shards to awaken the monster.")]
    public int ActivationsToAwake;
    private bool alertInit = false;
    private bool patrolInit = false;
    private bool chaseInit = false;
    private bool distractInit = false;
    private bool _statue;
    private DungeonManager dungeonManager;
    private Player playerObj;
    void Start()
    {
        _statue = Statue;
        // If statue start distracted otherwise patrol
        if(_statue) currentState = State.Distracted;
        else currentState = State.Patrolling;
        //currentState = State.Patrolling;
        dungeonManager = FindObjectOfType<DungeonManager>();
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
                distractInit = false;
                if (!patrolInit)
                {
                    //Debug.Log("Patrol Start");
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
                    //Debug.Log("Alert Start");
                    alert.Init();
                    alertInit = true;
                }
                if (alert != null) { alert.On_Update(); }
                break;
            case State.Chasing:
                
                if (playerObj.activeSafeZones.Count > 0)
                {
                    currentState = State.Alert;
                    patrolInit = false;
                    chaseInit = false;
                    if (!alertInit)
                    {
                        alert.Init();
                        alertInit = true;
                    }
                    if (alert != null) { alert.On_Update(); }
                    break;
                }
                
                patrolInit = false;
                alertInit = false;
                distractInit = false;
                if (!chaseInit)
                {
                    //Debug.Log("Chase Start");
                    SetChase();
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
                    //Debug.Log("Distract Start");
                    distracted.Init();
                    distractInit = true;
                }
                if (distracted != null) { distracted.On_Update(); }
                break;
        }
    }

    //Returns if monster is currently a statue
    public bool IsStatue() => _statue;

    //awakes monster from statue
    public void AwakenStatue() => _statue = false;
    
    //Returns amount of switch activations to awaken statue
    public int GetActivationsToAwake() => ActivationsToAwake;

    //Returns the statue sprite
    public Sprite GetStatueSprite() => SpriteStatue;
    
    private void SetChase(){
        if (dungeonManager != null)
        {
            dungeonManager.AddEnemy(this);
        }
    }
    
    private void StopChase(){
        if (dungeonManager != null)
        {
            dungeonManager.RemoveEnemy(this);
        }
    }
}

