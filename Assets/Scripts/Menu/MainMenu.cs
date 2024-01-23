using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class MainMenu : MonoBehaviour
{
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.5f;

    //Functions ======================================================================================
    public void PlayGame()
    {
        StartCoroutine(FadeToBlackAndLoadScene());
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    IEnumerator FadeToBlackAndLoadScene()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        Loader.Load(Loader.Scene.GameScene); // Replace with your scene loading logic
    }

    IEnumerator FadeToBlack()
    {
        fadeOutUIImage.gameObject.SetActive(true); // Activate the image if it's not already
        Color objectColor = fadeOutUIImage.color;
        float fadeAmount;

        while (fadeOutUIImage.color.a < 1.5)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

}
