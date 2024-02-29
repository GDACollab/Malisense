using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBeast_noiseDetect : MonoBehaviour
{
    //public StateMachine stateMachine;
    //How many sounds can occur in the monster's vicinity before it starts chasing you
    public int noiseSensitivity = 1;
    int noiseCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When the gameObject entes another gameObject with the 'isTrigger' in a collider2D turned on
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.name + "!");
        if (collision.name == "Noise Object(Clone)") //Consider changing to detect via tags instead of names. If that breaks the program its on you /j
        {
            //check radius for noise level
            Debug.Log("hit sound object");

        }
        /***
        if (noiseCounter < noiseSensitivity)
        {
            stateMachine.currentState = StateMachine.State.Patrolling;
        }
        ***/
    }
}
