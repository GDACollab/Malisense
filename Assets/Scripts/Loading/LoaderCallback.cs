using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.5f;

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
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values

        while (fadeOutUIImage.color.a > 0)
        {
            objectColor.a -= fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        fadeOutUIImage.gameObject.SetActive(false);

        // Set the delay duration (in seconds)
        float delayDuration = 5.0f;

        // Wait for the specified duration
        yield return new WaitForSeconds(delayDuration);

        // Call the loader callback after the delay
        Loader.LoaderCallback();
    }

}
