using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random=UnityEngine.Random;


[RequireComponent(typeof(Light2D))]
public class ActiveLight : MonoBehaviour
{
    public Vector2 flickerPercentRange = new Vector2(0.85f, 0.95f);
    public Vector2 dimPercentRange = new Vector2(0.6f, 0.7f);
    public Vector2 flickerTimeRange = new Vector2(0.15f, 0.25f);
    
    private float flickerPercent;
    private float dimPercent;
    private float flickerTime;
    private float timeElapsed = 0f;
    private bool shrinking = true;
    
    private Light2D lightSource;
    private Vector3 initialLightSize;
    private Vector3 endLightSize;
    private float initialLightIntensity;
    private float endLightIntensity;
    
    // Start is called before the first frame update
    void Start()
    {
        flickerPercent = Random.Range(flickerPercentRange.x, flickerPercentRange.y);
        dimPercent = Random.Range(dimPercentRange.x, dimPercentRange.y);
        flickerTime = Random.Range(flickerTimeRange.x, flickerTimeRange.y);
        lightSource = GetComponent<Light2D>();
        initialLightSize = transform.localScale;
        endLightSize = new Vector3(flickerPercent*initialLightSize.x, flickerPercent*initialLightSize.y, initialLightSize.z);
        initialLightIntensity = lightSource.intensity;
        endLightIntensity = initialLightIntensity*dimPercent;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeElapsed<flickerTime){
            if(shrinking){
                transform.localScale = Vector3.Lerp(initialLightSize, endLightSize, timeElapsed/flickerTime);
                lightSource.intensity = Mathf.Lerp(initialLightIntensity, endLightIntensity, timeElapsed/flickerTime);
            }
            else{
                transform.localScale = Vector3.Lerp(endLightSize, initialLightSize, timeElapsed/flickerTime);
                lightSource.intensity = Mathf.Lerp(endLightIntensity, initialLightIntensity, timeElapsed/flickerTime);
            }
            timeElapsed += Time.deltaTime;
        }
        else{
            transform.localScale = (shrinking) ? endLightSize : initialLightSize;
            shrinking = !shrinking;
            flickerTime = Random.Range(flickerTimeRange.x, flickerTimeRange.y);
            if(shrinking){
                flickerPercent = Random.Range(flickerPercentRange.x, flickerPercentRange.y);
                dimPercent = Random.Range(dimPercentRange.x, dimPercentRange.y);
                endLightSize = new Vector3(flickerPercent*initialLightSize.x, flickerPercent*initialLightSize.y, initialLightSize.z);
                endLightIntensity = initialLightIntensity*dimPercent;
            }
            timeElapsed = 0f;
        }
    }
}
