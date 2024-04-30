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
    private static bool isDone = false;
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.1f;
    static AsyncOperation operation;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Debug.Log("Doing routine");
            StartCoroutine(LoadingDelay());
        }
        else if (isDone)
        {
            Debug.Log("Is Done");
            isDone = false;
            isFirstUpdate = true;
        }
        else
        {
            if (operation != null && operation.isDone)
            {
                operation = null;
                StartCoroutine(LoadNextScene());
            }
            else
            {
                StartCoroutine(WaitForASecond());
            }
        }
    }

    private IEnumerator LoadingDelay()
    {
        yield return FadeToBlack(); // Fades current scene to black
        //fadeOutUIImage.gameObject.SetActive(false);

        // Wait for the specified duration
        //yield return new WaitForSeconds(5.0f);
        yield return StartCoroutine(WaitOnLoadScene());

        yield return StartCoroutine(LoadNextScene());

        // Call the loader callback after the delay
        isDone = true;
        Debug.Log("Trying to callback");
        Loader.LoaderCallback();
    }

    // Fades current scene to black in preperation for the next Scene
    private IEnumerator FadeToBlack()
    {
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        while (fadeOutUIImage.color.a > 0)
        {
            Debug.Log("Fading");
            objectColor.a -= fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

    // Loads the Next Scene
    private IEnumerator LoadNextScene()
    {
        Debug.Log("Attempt to load next");
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        fadeOutUIImage.gameObject.SetActive(true);
        while (fadeOutUIImage.color.a < 1.5)
        {
            Debug.Log("Loading Next");
            objectColor.a += fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        isDone = true;
        Debug.Log("Trying to callback");
        Loader.LoaderCallback();
    }

    // Waits on LoadingScreen for as long as it needs to
    private IEnumerator WaitOnLoadScene()
    {
        operation = SceneManager.LoadSceneAsync((int)Loader.GetNextScene());
        fadeOutUIImage.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            Debug.Log("waiting on screen");
            yield return null;
        }
        Debug.Log("Finished Waiting on Screen");
    }

    private IEnumerator WaitForASecond()
    {
        yield return new WaitForSeconds(0.1f);
    }

}
