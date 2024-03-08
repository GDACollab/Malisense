using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{   
    public enum Scene
    {
        MainMenu = 0, // This refers to the scene in Build Settings at index 0
        LoadingScene = 1, // This refers to the scene in Build Settings at index 1
        Village = 2, // This refers to the scene in Build Settings at index 2
        Dungeon = 3, // This refers to the scene in Build Settings at index 3
        DeathScene = 4, // This refers to the scene in Build Settings at index 4
    }

    private static Action onLoaderCallback;

    public static void Initialize()
    {
        SceneManager.LoadScene((int)Scene.LoadingScene);
    }

    public static void Load(Scene scene)
    {
        // Action is stored, then is called from function LoaderCallback
        onLoaderCallback = () =>
        {
            SceneManager.LoadScene((int)scene); // USES BUILD SETTINGS INDEX, NOT NAME 
        };

        // Load Loading Scene which calls on the function that runs Action
        SceneManager.LoadScene((int)Scene.LoadingScene);
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
