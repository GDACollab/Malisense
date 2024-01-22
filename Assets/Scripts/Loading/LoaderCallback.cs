using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            StartCoroutine(LoadingDelay());
        }
    }

    private IEnumerator LoadingDelay()
    {
        // Set the delay duration (in seconds)
        float delayDuration = 5.0f;

        // Wait for the specified duration
        yield return new WaitForSeconds(delayDuration);

        // Call the loader callback after the delay
        Loader.LoaderCallback();
    }

}
