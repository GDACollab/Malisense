using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using FMODUnity;


public class DeathScript : MonoBehaviour
{
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.07f;

    [SerializeField] private EventReference ascension;
    //Functions ======================================================================================
    public void ResetButton()
    {
        playSound();
        StartCoroutine(Fader());
    }

    IEnumerator Fader()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        //Play Audio with FMOD - Need to research Library

        yield return StartCoroutine(ImageFade());
        Loader.Load(Loader.Scene.GameScene); 
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
    private void playSound()
    {
        AudioManager.Instance.PlayOneShot(ascension, this.transform.position);
    }
}
