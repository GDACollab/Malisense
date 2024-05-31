using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarnelianEarthquake : MonoBehaviour, ISwitchable
{
    [SerializeField][Tooltip("Screen shake duration")] float quakeDuration = 1.5f;
    [SerializeField][Tooltip("Screen shake intensity")] float quakeIntensity = 0.6f;

    public void SwitchInit(bool activated) { }
    public void SwitchInteract(bool activated)
    {
        foreach (PlayerInput input in FindObjectsOfType<PlayerInput>())
        {
            input.enabled = false;
        }

        StartCoroutine(WaitToEnableInput(0.5f, quakeDuration + 0.5f));
    }

    private IEnumerator WaitToEnableInput(float waitBefore, float waitTime)
    {
        yield return new WaitForSeconds(waitBefore);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmartCamera>().ScreenShake(quakeDuration, quakeIntensity);
        yield return new WaitForSeconds(waitTime);
        foreach (PlayerInput input in FindObjectsOfType<PlayerInput>())
        {
            input.enabled = true;
        }
    }
}
