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
    public AudioManager audioManager;
    void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Global Teapot").GetComponent<AudioManager>();
        audioManager.PlayMenuOST();
    }
    //Functions ======================================================================================
    public void PlayGame()
    {
        StartCoroutine(FadeToBlackAndLoadScene());
    }
    
    public void SkipVillage()
    {
        StartCoroutine(FadeToBlackAndLoadDungeon());
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void volumeChange(float sliderVolume)
    {
        audioManager.masterVolume = sliderVolume;
    }
    IEnumerator FadeToBlackAndLoadScene()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        Loader.Load(Loader.Scene.Village); 
    }
    
    IEnumerator FadeToBlackAndLoadDungeon()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        Loader.Load(Loader.Scene.Dungeon_F1);
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
