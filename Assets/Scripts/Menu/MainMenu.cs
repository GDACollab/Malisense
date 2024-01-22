using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class MainMenu : MonoBehaviour
{


    //Functions ======================================================================================
    public void PlayGame()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    
    
}
