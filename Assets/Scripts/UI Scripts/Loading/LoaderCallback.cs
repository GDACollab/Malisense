using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private static bool isFirstUpdate = true;
    private static bool isDone = false; // Ensures everything is reset after finishing
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.1f;
    static AsyncOperation operation;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            StartCoroutine(LoadingDelay());
        }
        else if (isDone)
        {
            operation = null;
            isDone = false;
            isFirstUpdate = true;
        }
        else
        {
            if (operation != null && operation.progress >= 0.9f)
            {
                StartCoroutine(LoadNextScene());
            }
        }
    }

    private IEnumerator LoadingDelay()
    {
        yield return FadeToBlack(); // Fades current scene to black

        // Wait for the next scene to finish properly loading
        yield return StartCoroutine(WaitOnLoadScene());
    }

    // Fades current scene to black in preperation for the next Scene
    private IEnumerator FadeToBlack()
    {
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        while (fadeOutUIImage.color.a > -2)
        {
            objectColor.a -= fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

    // Loads the Next Scene
    private IEnumerator LoadNextScene()
    {
        
        // Attempting to fade Loading screen to black (currently fails)
        yield return StartCoroutine(FadeToBlack());
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        fadeOutUIImage.gameObject.SetActive(true);
        //Debug.Log(fadeOutUIImage.color.a);
        while (fadeOutUIImage.color.a < 1.5)
        {
            objectColor.a += fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }

        // Call the loader callback after the delay
        isDone = true;
        Loader.LoaderCallback();
    }

    // Waits on LoadingScreen for as long as it needs to
    private IEnumerator WaitOnLoadScene()
    {
        Debug.Log(Loader.GetNextScene());
        operation = SceneManager.LoadSceneAsync((int)Loader.GetNextScene());
        fadeOutUIImage.gameObject.SetActive(true);
        while (operation.progress <= 0.9)
        {
            yield return null;
        }
    }
}
