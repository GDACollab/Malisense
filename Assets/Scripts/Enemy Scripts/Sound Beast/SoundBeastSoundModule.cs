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
                if (machine.currentState == StateMachine.State.Chasing) { GetComponent<SoundBeastChase>().CancelInvoke(); }
                machine.currentState = StateMachine.State.Distracted;
            }
            else if (playerObj.activeSafeZones.Count <= 0)
            {
                if (isCircling && machine.currentState == StateMachine.State.Alert)
                {
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