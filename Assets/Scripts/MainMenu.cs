using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System;


public class MainMenu : MonoBehaviour
{


    //Functions ======================================================================================
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    
    
}
