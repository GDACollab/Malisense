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
    
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    
    void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Global Teapot").GetComponent<AudioManager>();
        audioManager.PlayMenuOST();
    }
    private void Start() {
        if(GlobalTeapot.Instance.currProgress == GlobalTeapot.TeaType.Intro){ newGameButton.SetActive(false); }

        masterSlider.value = audioManager.masterVolume;
        musicSlider.value = audioManager.musicVolume;
        sfxSlider.value = audioManager.sfxVolume;

        StartCoroutine(GlobalTeapot.Instance.fader.FadeFromBlack(2));
    }
    //Functions ======================================================================================
    public void PlayGame()
    {
        StartCoroutine(FadeToBlackAndLoadScene());
    }
    
    public void RestartGame()
    {
        GlobalTeapot.Instance.BrewNewTea();
        PlayGame();
    }
    
    public void SkipVillage()
    {
        StartCoroutine(FadeToBlackAndLoadDungeon());
    }

    public void ShowCredits()
    {
        StartCoroutine(FadeToBlackAndLoadCredits());
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void masterVolumeChange(float sliderVolume)
    {
        audioManager.masterVolume = sliderVolume;
    }
    
    public void musicVolumeChange(float sliderVolume)
    {
        audioManager.musicVolume = sliderVolume;
    }
    
    public void sfxVolumeChange(float sliderVolume)
    {
        audioManager.sfxVolume = sliderVolume;
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
    
    IEnumerator FadeToBlackAndLoadCredits()
    {
        fadeOutUIImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        Loader.Load(Loader.Scene.Credits, true);
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
