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
        Level_Select = 3,
        Dungeon_F1 = 4, // This refers to the scene in Build Settings at index 3
        Dungeon_F2 = 5, // This refers to the scene in Build Settings at index 3
        DeathScene = 6, // This refers to the scene in Build Settings at index 4
    }
    private delegate AsyncOperation AsyncLoaderCallback();
    private static AsyncLoaderCallback onLoaderCallback;
    private static Scene currentScene = Scene.MainMenu;

    public static void Initialize()
    {
        SceneManager.LoadScene((int)Scene.LoadingScene);
    }

    public static void Load(Scene scene)
    {
        // Action is stored, then is called from function LoaderCallback
        onLoaderCallback = () =>
        {
            currentScene = scene;
            var asyncScene = SceneManager.LoadSceneAsync((int)scene); // USES BUILD SETTINGS INDEX, NOT NAME 
            asyncScene.allowSceneActivation = false;
            return asyncScene;
        };
        
        currentScene = Scene.LoadingScene;
        // Load Loading Scene which calls on the function that runs Action
        SceneManager.LoadScene((int)Scene.LoadingScene);
    }

    public static AsyncOperation LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            var asyncScene = onLoaderCallback();
            onLoaderCallback = null;
            return asyncScene;
        }
        return null;
    }
    
    public static Scene GetCurrentScene(){
        
        return currentScene;
    }
}
