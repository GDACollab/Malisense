using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBeast_noiseDetect_copy : StateBaseClass
{
    public StateMachine_Improved machine;
   

    public override void Init()
    {
        machine = GetComponent<StateMachine_Improved>();
    }

    public override void On_Update()
    {
        
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

            // Check radius for noise level
            float loudness = noise.diameter;
            Vector2 noisePos = noise.transform.position;
            if (noise.parent.tag == "Player" && machine.currentState == StateMachine_Improved.State.Patrolling)
            {
                machine.switchState(StateMachine_Improved.State.Alert);
            }
        }
    }
}