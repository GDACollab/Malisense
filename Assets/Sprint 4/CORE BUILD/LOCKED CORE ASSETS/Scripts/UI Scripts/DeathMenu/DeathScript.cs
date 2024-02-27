using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class DeathScript : MonoBehaviour
{
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.5f;

    //Functions ======================================================================================
    public void ResetButton()
    {
        StartCoroutine(Fader());
    }

    IEnumerator Fader()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        //Play Audio with FMOD - Need to research Library

        yield return StartCoroutine(ImageFade());
        Loader.Load(Loader.Scene.Dungeon); 
    }

    IEnumerator ImageFade()
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
