using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{

    private float pulseTime = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        pulseTime += Time.deltaTime;
        if(pulseTime > 2.0f) Destroy(gameObject);   
    }
}
