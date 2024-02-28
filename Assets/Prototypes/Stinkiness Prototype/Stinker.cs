/* Stinker.cs
 * Evan Lake
 * 
 * controls stink values and interaction between stinkable objects
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinker : MonoBehaviour
{
    
    private float maxStink = 100;
    private float minStink = 0;
    [Header("Stink Setting")]
    [SerializeField]
    private float stinkPerSec = -5; // base change in stink per second

    [Header("Stink Emmission Settings")]
    [SerializeField]
    private bool isStinkEmitter = true; // determines if object should create and spread new stink
    [SerializeField]
    private float stinkEmitPerSec = 15; // how fast object spreads stink to collided objects

    [Header("Stink Status")]
    [SerializeField]
    private float stink = 0; // current stink saturation value (minStink to maxStink)


    // Start is called before the first frame update
    void Start()
    {
        // checking dependencies
        if (GetComponent<Rigidbody2D>() == null) Debug.LogError("StinkerError: no rigidbody 2d detected!\n");
        if (GetComponent<BoxCollider2D>() == null) Debug.LogError("StinkerError: no box collider 2d detected!\n");

        // if the sleepmode isn't NeverSleep then the stink spread, using OnTriggerStay2D, stops after a small time
        if (isStinkEmitter) {
            GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        modStink(stinkPerSec * Time.fixedDeltaTime);

    }

    public float getStink() {
        return stink;
    }

    public void setStink(float new_stink) {
        if (new_stink >= minStink && new_stink <= maxStink) {
            stink = new_stink;
        } else {
            Debug.LogError("StinkerError: " + gameObject.name + ": setStink(): new_stink out of bounds (" + new_stink + ")\n");
        }
        return;
    }

    // changes the stink value by mod_stink as long as it doesn't cross max/min stink
    public float modStink(float mod_stink) {
        float result = stink + mod_stink;
        if (result < minStink) result = minStink;
        else if (result > maxStink) result = maxStink;
        setStink(result);
        return stink;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!isStinkEmitter) return;
        Stinker otherStink = collision.gameObject.GetComponent<Stinker>();
        if (otherStink != null) {
            float dif = stink - otherStink.getStink();
            if (dif > 0) {
                // increase the stink of the other stinker, modulo by the dif in case dif is less then stinkSpreadPerSec
                otherStink.modStink((stinkEmitPerSec % dif) * Time.deltaTime);
            }
            
        }
    }

}
