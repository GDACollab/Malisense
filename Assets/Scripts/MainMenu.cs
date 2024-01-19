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
        
    }

    public void Credits()
    {


    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    
}
