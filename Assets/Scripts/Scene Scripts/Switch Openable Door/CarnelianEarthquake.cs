using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarnelianEarthquake : MonoBehaviour, ISwitchable
{
    [SerializeField] [Tooltip("Screen shake duration")] float quakeDuration = 1.5f;
    [SerializeField] [Tooltip("Screen shake intensity")] float quakeIntensity = 0.6f;
    
    public void SwitchInit(bool activated){}
    public void SwitchInteract(bool activated){
        foreach (PlayerInput input in FindObjectsOfType<PlayerInput>())
        {
            input.enabled = false;
        }
        
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmartCamera>().ScreenShake(quakeDuration, quakeIntensity);
        StartCoroutine(WaitToEnableInput(quakeDuration+0.5f));
    }
    
    private IEnumerator WaitToEnableInput(float waitTime){
        yield return new WaitForSeconds(waitTime);
        foreach (PlayerInput input in FindObjectsOfType<PlayerInput>())
        {
            input.enabled = true;
        }
    }
}
