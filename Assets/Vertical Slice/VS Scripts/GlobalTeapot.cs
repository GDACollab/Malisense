using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GlobalTeapot : MonoBehaviour
{
    // A Very Special Teapot
    public static GlobalTeapot Instance { get; private set; }
    
    public int villageInk = 0;
    public AudioManager audioManager;
    public Loader.Scene currentScene = Loader.Scene.DeathScene;
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
        audioManager = GetComponent<AudioManager>();
    }
    
    private void Update() {
        currentScene = Loader.GetCurrentScene();
    }
}
