using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBeast_noiseDetect_copy : MonoBehaviour
{

    public StateMachine_Improved machine;

    // Start is called before the first frame update
    void Start()
    {
        machine = GetComponent<StateMachine_Improved>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When the gameObject entes another gameObject with the 'isTrigger' in a collider2D turned on
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collided with " + collision.name + "!");
        if (collision.tag == "NoiseObject")
        {
            // get the noise object and check that it has the noiseobject script
            scr_noiseObject noise = collision.GetComponent<scr_noiseObject>();
            if (noise == null) {
                Debug.LogError("ERROR: could not find scr_noiseObject in detected noise object \"" + collision.name + "\"");
                return;
            }

            //Debug.Log("noiseObject hit: " + noise.name);
            //check radius for noise level
            float loudness = noise.diameter;
            Vector2 noisePos = noise.transform.position;
            if (noise.parent.tag == "Player") {
                machine.switchState(StateMachine_Improved.State.Alert);
            }


        }
    }
}
