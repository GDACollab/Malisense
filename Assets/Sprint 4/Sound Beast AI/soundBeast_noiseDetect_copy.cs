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

    //When the gameObject enters another gameObject with the 'isTrigger' in a collider2D turned on
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NoiseObject")
        {
            // Get the noise object and check that it has the noiseobject script
            scr_noiseObject noise = collision.GetComponent<scr_noiseObject>();
            if (noise == null) {
                Debug.LogError("ERROR: could not find scr_noiseObject in detected noise object \"" + collision.name + "\"");
                return;
            }

            // Check radius for noise level
            float loudness = noise.diameter;
            Vector2 noisePos = noise.transform.position;
            if (noise.parent.tag == "Player") {
                machine.switchState(StateMachine_Improved.State.Alert);
                //machine.switchState(StateMachine_Improved.State.Patrolling);
            }
        }
    }
}