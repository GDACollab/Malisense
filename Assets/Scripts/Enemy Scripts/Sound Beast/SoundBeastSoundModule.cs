using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(StateMachine))]
public class SoundBeastSoundModule : MonoBehaviour
{
    public bool heardSound = false;
    public bool isCircling = false;
    public Vector3 detectedNoisePos;
    
    
    private StateMachine machine;
    private Player playerObj;
    
    private void Awake() 
    {
        machine = GetComponent<StateMachine>();
        playerObj = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    
    // private void SetCircling(bool circling){
    //     isCircling = true;
    // }
    
    // private void Awake()
    // {
    //     patrolPath = GetComponent<PatrolPath>();
    // }

    // private SBProtoPatrolArea GetRandomArea()
    // {
    //     var areas = patrolPath.areas.Where(area => !area.Contains(transform.position));
    //     if(areas.Count() == 0)
    //     {
    //         areas = patrolPath.areas;
    //     }

    //     float maxWeight = areas.Sum(area => area.weight);
    //     float value = maxWeight * Random.value;

    //     foreach (var area in areas)
    //     {
    //         value -= area.weight;
    //         if (value <= 0)
    //             return area;
    //     }

    //     return areas.First();
    // }

    // private SBProtoPatrolArea GetNextArea()
    // {
    //     return patrolPath.areas[_pathIndex++ % patrolPath.areas.Length];
    // }

    // public override void Init()
    // {
    //     aiPath = GetComponent<AIPath>();
    //     machine = GetComponent<StateMachine>();
    //     aiPath.destination = transform.position;
    //     aiPath.maxSpeed = maxSpeed;
    //     aiPath.SearchPath();
    //     _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

    //     // Start at the closest path point when entering patrol state
    //     var startArea = patrolPath.FindClosestArea(transform.position);
    //     _pathIndex = Array.IndexOf(patrolPath.areas, startArea);
    //     playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    // }

    // public override void On_Update()
    // {
    //     if(aiPath.reachedEndOfPath)
    //     {
    //         // Currently idle
    //         _idleTimeLeft -= Time.deltaTime;

    //         // When done idling, move to new area
    //         if(_idleTimeLeft < 0f)
    //         {
    //             _idleTimeLeft = Random.Range(minIdleTime, maxIdleTime);

    //             if (patrolPath && patrolPath.areas.Length > 0)
    //             {
    //                 SBProtoPatrolArea nextArea = randomPath ? GetRandomArea() : GetNextArea();
    //                 aiPath.destination = nextArea.GetRandomPoint();
    //                 aiPath.SearchPath();
    //             }
    //         }
    //     }
    // }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject")
        {
            // Get the noise object and check that it has the noiseobject script
            scr_noiseObject noise = collision.GetComponent<scr_noiseObject>();
            if (noise == null)
            {
                Debug.LogError("ERROR: could not find scr_noiseObject in detected noise object \"" + collision.name + "\"");
                return;
            }
            heardSound = true;

            // Check radius for noise level
            float loudness = noise.diameter;
            detectedNoisePos = noise.transform.position;
            Debug.Log(noise.parent.name);
            
            if (noise.noiseDistractsSound == true)
            {
                Debug.Log("Distracted!");
                if(machine.currentState == StateMachine.State.Chasing){GetComponent<SoundBeastChase>().CancelInvoke();}
                machine.currentState = StateMachine.State.Distracted;
            }
            else if(playerObj.activeSafeZones.Count <= 0){
                if(isCircling && machine.currentState == StateMachine.State.Alert){
                    isCircling = false;
                    GetComponent<AIPath>().enabled = false;
                    machine.currentState = StateMachine.State.Chasing;
                }
                else if (machine.currentState == StateMachine.State.Patrolling)
                {
                    machine.currentState = StateMachine.State.Alert;
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject")
        {
            heardSound = false;
        }
    }
    
    
}