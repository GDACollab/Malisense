using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    //Variables ======================================================================================
    public Image fadeImage; // Reference to the Image used for fading
    public float fadeSpeed = 0.8f;

    //Functions ======================================================================================
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsMenu()
    {
        StartCoroutine(FadeToOptions());
    }

    public void Credits()
    {


    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    //Coroutines ======================================================================================
    IEnumerator FadeToOptions()
    {
        // Gradually increase the alpha value of the fade image
        float alpha = fadeImage.color.a;
        while (alpha < 1f)
        {
            alpha += fadeSpeed * Time.deltaTime;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        // Enable Game Object Here
    }
}
