using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image fadeUIImage;
    
    public void Init(){
        fadeUIImage = GetComponentInChildren<Image>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += (_,_) => fadeUIImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeFromBlack(float fadeInTime)
    {
        Time.timeScale = 0f;
        fadeUIImage.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.1f);
        Color objectColor = fadeUIImage.color; //Gets Object Color and Modifies values
        objectColor.a = 1f;
        fadeUIImage.color = objectColor;
        float timer = fadeInTime;
        while (fadeUIImage.color.a > 0)
        {
            timer -= Time.unscaledDeltaTime;
            objectColor.a = Mathf.Lerp(-0.1f, 1, timer / fadeInTime);
            fadeUIImage.color = objectColor;
            yield return null;
        }
        fadeUIImage.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public IEnumerator FadeToBlack(Action sceneChange, float fadeOutTime)
    {
        Time.timeScale = 0f;
        Color objectColor = fadeUIImage.color; //Gets Object Color and Modifies values
        objectColor.a = 0;
        fadeUIImage.color = objectColor;
        fadeUIImage.gameObject.SetActive(true);
        float timer = fadeOutTime;
        while (fadeUIImage.color.a < 1)
        {
            timer -= Time.unscaledDeltaTime;
            objectColor.a = Mathf.Lerp(1.1f, 0, timer / fadeOutTime);
            fadeUIImage.color = objectColor;
            yield return null;
        }
        Time.timeScale = 1f;
        sceneChange();
        yield return new WaitForSecondsRealtime(0.1f);
    }
}
