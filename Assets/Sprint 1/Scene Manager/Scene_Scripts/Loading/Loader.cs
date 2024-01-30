using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{   
    public enum Scene
    {
        MainMenu,
        LoadingScene,
        GameScene
    }

    private static Action onLoaderCallback;
    public static void Load(Scene scene)
    {
        //Action is stored, then is called from function LoaderCallback
        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        //Load Loading Scene which calls on the function that runs Action
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
