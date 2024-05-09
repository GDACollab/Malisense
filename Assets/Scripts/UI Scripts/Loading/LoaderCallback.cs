using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.1f;
    private Color objectColor;
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            StartCoroutine(LoadingDelay());
            objectColor = fadeOutUIImage.color;
        }
    }

    private IEnumerator LoadingDelay()
    {
        //Gets Object Color and Modifies values

        objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values


        yield return StartCoroutine(FadeToBlackToLoadScene());

        // Set the delay duration (in seconds)
        float delayDuration = 0.1f;

        // Wait for the specified duration
        yield return new WaitForSeconds(delayDuration);

        yield return StartCoroutine(FadeToBlackToNextScene());

        // Call the loader callback after the delay
        Loader.LoaderCallback();
    }
    private IEnumerator FadeToBlackToLoadScene()
    {
        while (fadeOutUIImage.color.a > 0)
        {
            objectColor.a -= fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        fadeOutUIImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeToBlackToNextScene()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        while (fadeOutUIImage.color.a < 1.5)
        {
            objectColor.a += fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

}

