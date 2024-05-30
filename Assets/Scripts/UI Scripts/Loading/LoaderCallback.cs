using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    public bool keepLoading = false;

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
        yield return StartCoroutine(FadeToBlackToLoadScene());

        var asyncScene = Loader.LoaderCallback(); // Calls to load the scene after load 

        while (asyncScene.progress<0.9f || keepLoading) // Waits on Loading screen while other screen loads
        {
            yield return null;
        }
        //fadeOutUIImage.gameObject.SetActive(false);
        yield return StartCoroutine(FadeToBlackToNextScene(asyncScene));
        // Call the loader callback after the delay

    }
    private IEnumerator FadeToBlackToLoadScene()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        while (fadeOutUIImage.color.a > 0)
        {
            objectColor.a -= fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

    private IEnumerator FadeToBlackToNextScene(AsyncOperation asyncScene)
    {
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        fadeOutUIImage.gameObject.SetActive(true);
        while (fadeOutUIImage.color.a < 1)
        {
            objectColor.a += fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        // fadeOutUIImage.gameObject.SetActive(false);
        asyncScene.allowSceneActivation = true;
    }
}

