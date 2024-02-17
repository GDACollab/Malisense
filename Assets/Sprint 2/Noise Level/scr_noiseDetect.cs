using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_noiseDetect : MonoBehaviour
{
    public StateMachine stateMachine;
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
            Debug.Log("hit a noise object");
            noiseCounter++; //The monster becomes more aware the more sounds it hears
            if(noiseCounter >= noiseSensitivity) //When it hears enough sounds, it becomes aware of the player
            {
                Debug.Log("The monster will enter chase mode now!");

                //When this if statement is entered, the monster will have realized the sound originates from the player, and begin chasing
                //It will attempt to move to the center location of whatever sound object hit it most recently.
                
                //Below is an attempt to make the monster target the location of the object. It may work, but you may need to write custom code instead.
                //GetComponent<EnemyMovement>().target = collision.transform; //Makes the enemy target the location the sound originated
                //stateMachine.currentState = StateMachine.State.Chasing;
                
            }
        }
        /***
        if (noiseCounter < noiseSensitivity)
        {
            stateMachine.currentState = StateMachine.State.Patrolling;
        }
        ***/
    }
}
