using System.Collections;
using System.Collections.Generic;
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
        //Gets Object Color and Modifies values


        yield return StartCoroutine(FadeToBlackToLoadScene());

        Loader.LoaderCallback();

        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }
        Debug.Log("Moved here");
        yield return StartCoroutine(FadeToBlackToNextScene());

        // Call the loader callback after the delay

    }
    private IEnumerator FadeToBlackToLoadScene()
    {
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
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
        Color objectColor = fadeOutUIImage.color; //Gets Object Color and Modifies values
        fadeOutUIImage.gameObject.SetActive(true);
        while (fadeOutUIImage.color.a < 1)
        {
            objectColor.a += fadeSpeed * Time.deltaTime;
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        fadeOutUIImage.gameObject.SetActive(false);
    }

}

