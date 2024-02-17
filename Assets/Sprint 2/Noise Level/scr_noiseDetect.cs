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
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OverlapCircle() > 0)
    }
    private bool OnCOllideTrue()
    {
           //enemy
           //if enemy touching sound
           //   debug.log
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with something!" + collision.name);
        if (collision.name == "Noise Object(Clone)")
        {
            Debug.Log("hit a noise object");
            noiseCounter++;
            if(noiseCounter >= noiseSensitivity)
            {
                Debug.Log("hope this works");
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
