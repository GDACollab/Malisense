using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    private bool effectActive = false;
    private float pulseTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

        gameObject.SetActive(false);
    }

    public void setPulseActive() {
        Debug.Log("Set pulse effect to active");
        effectActive = true; }

    // Update is called once per frame
    void Update()
    {
        if (effectActive)
        {
            pulseTime += Time.deltaTime;
        }
        gameObject.SetActive(effectActive);
        if(pulseTime > 2.0f)
        {
            effectActive = false;
            pulseTime = 0.0f;
            gameObject.SetActive(false);
        }
    }
}
