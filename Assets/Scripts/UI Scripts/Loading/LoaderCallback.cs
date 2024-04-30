using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.1f;

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
        yield return FadeToBlack(); // Fades current scene to black
        fadeOutUIImage.gameObject.SetActive(false);

        // Wait for the specified duration
        yield return new WaitForSeconds(5.0f);
        //yield return WaitOnLoadScene();

        yield return StartCoroutine(LoadNextScene());

        // Call the loader callback after the delay
        Loader.LoaderCallback();
    }

    // Fades current scene to black in preperation for the next Scene
    private IEnumerator FadeToBlack()
    {
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        while (fadeOutUIImage.color.a > 0)
        {
            objectColor.a -= fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

    // Loads the Next Scene
    private IEnumerator LoadNextScene()
    {
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        fadeOutUIImage.gameObject.SetActive(true);
        while (fadeOutUIImage.color.a < 1.5)
        {
            objectColor.a += fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

    // Waits on LoadingScreen for as long as it needs to
    private IEnumerator WaitOnLoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)Loader.Scene.LoadingScene);
        fadeOutUIImage.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

}
