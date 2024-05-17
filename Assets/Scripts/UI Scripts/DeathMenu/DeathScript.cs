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

    //Start of Tristyn's changes
    //public Text titleText;
    public Button ariseButton;

    //Functions ======================================================================================
    public void ResetButton()
    {
        StartCoroutine(Fader());
    }

    IEnumerator Fader()
    {
        //
        //titleText.gameObject.SetActive(false);
        ariseButton.gameObject.SetActive(false);
        //
        fadeOutUIImage.gameObject.SetActive(true);
        //Play Audio with FMOD - Need to research Library

        yield return StartCoroutine(ImageFade());
        Loader.Load(Loader.Scene.Village); 
    }

    IEnumerator ImageFade()
    {
        fadeOutUIImage.gameObject.SetActive(true); // Activate the image if it's not already
        Color objectColor = fadeOutUIImage.color;
        float fadeAmount;

        while (fadeOutUIImage.color.a < 1.0)//1.5
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
    }

}
